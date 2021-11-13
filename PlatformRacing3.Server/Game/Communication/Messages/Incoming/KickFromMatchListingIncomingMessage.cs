using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming;

internal class KickFromMatchListingIncomingMessage : MessageIncomingJson<JsonKickFromMatchListingIncomingMessage>
{
	internal override void Handle(ClientSession session, JsonKickFromMatchListingIncomingMessage message)
	{
		if (!session.IsLoggedIn)
		{
			return;
		}

		session.LobbySession?.MatchListing?.Kick(session, message.SocketId);
	}
}