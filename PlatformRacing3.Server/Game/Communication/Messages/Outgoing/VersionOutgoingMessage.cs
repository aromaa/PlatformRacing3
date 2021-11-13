using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

internal class VersionOutgoingMessage : JsonOutgoingMessage<JsonVersionOutgoingMessage>
{
	internal VersionOutgoingMessage(uint version) : base(new JsonVersionOutgoingMessage(version))
	{
	}
}