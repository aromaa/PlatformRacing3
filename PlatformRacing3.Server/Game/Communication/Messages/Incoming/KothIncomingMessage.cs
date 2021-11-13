using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming;

internal class KothIncomingMessage : MessageIncomingJson<JsonKothIncomingMessage>
{
	internal override void Handle(ClientSession session, JsonKothIncomingMessage message)
	{
		session.MultiplayerMatchSession?.MatchPlayer?.Match.KothTime(session, message.Time);
	}
}