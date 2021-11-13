using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

internal sealed class JsonMatchFailedOutgoingMessage : JsonPacket
{
	private protected override string InternalType => "matchFailed";
}