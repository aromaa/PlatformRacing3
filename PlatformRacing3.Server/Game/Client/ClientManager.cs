using System.Collections.Concurrent;
using PlatformRacing3.Common.Level;
using PlatformRacing3.Common.User;
using PlatformRacing3.Server.Collections;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

namespace PlatformRacing3.Server.Game.Client;

internal sealed class ClientManager : IDisposable
{
	private const uint TimeoutTime = 10;

	private readonly ClientSessionCollection ClientsBySocketId;
	private readonly ConcurrentDictionary<uint, ClientSession> ClientsByUserId;

	public ClientManager()
	{
		this.ClientsBySocketId = new ClientSessionCollection(this.OnAdded, this.OnRemoved);
		this.ClientsByUserId = new ConcurrentDictionary<uint, ClientSession>();

		_ = this.CheckForTimedOutConnections();
	}

	internal int Count => this.ClientsBySocketId.Count;
	internal ICollection<ClientSession> LoggedInUsers => this.ClientsBySocketId.Sessions;

	internal void Add(ClientSession session)
	{
		this.ClientsBySocketId.TryAdd(session);
	}

	internal bool TryGetClientSessionBySocketId(uint socketId, out ClientSession session) => this.ClientsBySocketId.TryGetValue(socketId, out session);
	internal bool TryGetClientSessionByUserId(uint userId, out ClientSession session) => this.ClientsByUserId.TryGetValue(userId, out session);

	internal ClientSession GetClientSessionByUsername(string username) => this.ClientsBySocketId.Sessions.FirstOrDefault(c => username.Equals(c.UserData?.Username, StringComparison.OrdinalIgnoreCase));

	private void OnAdded(ClientSession session)
	{
		if (!session.IsGuest)
		{
			this.ClientsByUserId.AddOrUpdate(session.UserData.Id, session, (oldKey, oldValue) =>
			{
				oldValue.Disconnect("Logged in from another location");

				return session;
			});
		}
	}

	private void OnRemoved(ClientSession session)
	{
		if (!session.IsGuest)
		{
			this.ClientsByUserId.TryRemove(new KeyValuePair<uint, ClientSession>(session.UserData.Id, session));
		}
	}

	private async Task CheckForTimedOutConnections()
	{
		PeriodicTimer timer = new(TimeSpan.FromSeconds(5));

		while (await timer.WaitForNextTickAsync())
		{
			foreach (ClientSession session in this.ClientsBySocketId.Sessions)
			{
				bool inDeathmatch = session.MultiplayerMatchSession is { Match.LevelData.Mode: LevelMode.Deathmatch };

				if (session.LastPing.Elapsed.TotalSeconds >= (inDeathmatch ? ClientManager.TimeoutTime : ClientManager.TimeoutTime * 3))
				{
					session.Disconnect("Timeout (No ping)");
				}
				else if (!session.UserData.IsGuest)
				{
					//Temp workaround to keep the user cache actively loaded
					_ = UserManager.TryGetUserDataByIdAsync(session.UserData.Id);
				}
			}
		}
	}

	public void Dispose()
	{
		this.ClientsBySocketId.SendAsync(new LogoutTriggerOutgoingMessage("Server was shutdown."));
	}
}