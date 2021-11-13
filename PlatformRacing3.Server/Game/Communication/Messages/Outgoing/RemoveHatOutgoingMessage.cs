using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

internal class RemoveHatOutgoingMessage : JsonOutgoingMessage<JsonRemoveHatOutgoingMessage>
{
	internal RemoveHatOutgoingMessage(uint id) : base(new JsonRemoveHatOutgoingMessage(id))
	{
	}
}