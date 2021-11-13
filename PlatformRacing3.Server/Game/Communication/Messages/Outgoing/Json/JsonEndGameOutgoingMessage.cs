using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

internal sealed class JsonEndGameOutgoingMessage : JsonPacket
{
	private protected override string InternalType => "endGame";
}