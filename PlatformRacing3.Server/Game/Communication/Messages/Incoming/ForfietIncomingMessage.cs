using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming;

internal class ForfietIncomingMessage : IMessageIncomingJson
{
	public void Handle(ClientSession session, JsonPacket message)
	{
		if (!session.IsLoggedIn)
		{
			return;
		}

		session.MultiplayerMatchSession?.MatchPlayer?.Match.Forfeit(session);
	}
}