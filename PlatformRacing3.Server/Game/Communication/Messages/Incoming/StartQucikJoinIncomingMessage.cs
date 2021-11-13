using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;
using PlatformRacing3.Server.Game.Lobby;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming;

internal sealed class StartQucikJoinIncomingMessage : IMessageIncomingJson
{
	private readonly MatchListingManager matchListingManager;

	public StartQucikJoinIncomingMessage(MatchListingManager matchListingManager)
	{
		this.matchListingManager = matchListingManager;
	}

	public void Handle(ClientSession session, JsonPacket message)
	{
		if (!session.IsLoggedIn)
		{
			return;
		}

		this.matchListingManager.StartQuickJoin(session);
	}
}