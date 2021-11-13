using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming;

internal class BanFromMatchListingIncomingMessage : MessageIncomingJson<JsonBanFromMatchListingIncomingMessage>
{
	internal override void Handle(ClientSession session, JsonBanFromMatchListingIncomingMessage message)
	{
		session.LobbySession?.MatchListing?.Ban(session, message.SocketId);
	}
}