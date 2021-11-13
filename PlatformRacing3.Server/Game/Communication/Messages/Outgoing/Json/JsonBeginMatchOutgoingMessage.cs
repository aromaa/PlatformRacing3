using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

internal sealed class JsonBeginMatchOutgoingMessage : JsonPacket
{
	private protected override string InternalType => "beginMatch";

	internal JsonBeginMatchOutgoingMessage()
	{

	}
}