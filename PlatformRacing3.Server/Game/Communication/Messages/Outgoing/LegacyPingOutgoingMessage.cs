using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

internal class LegacyPingOutgoingMessage : JsonOutgoingMessage<JsonLegacyPingOutgoingMessage>
{
	internal LegacyPingOutgoingMessage(ulong time, ulong serverTime) : base(new JsonLegacyPingOutgoingMessage(time, serverTime))
	{
	}
}