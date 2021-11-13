using PlatformRacing3.Server.Core;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming;

internal class LegacyPingIncomingMessage : MessageIncomingJson<JsonLegacyPingIncomingMessage>
{
	internal override void Handle(ClientSession session, JsonLegacyPingIncomingMessage message)
	{
		session.LastPing.Restart();
		session.SendPacket(new LegacyPingOutgoingMessage(message.Time, (ulong)PlatformRacing3Server.Uptime.TotalSeconds));
	}
}