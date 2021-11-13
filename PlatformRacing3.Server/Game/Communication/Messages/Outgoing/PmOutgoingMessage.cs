using PlatformRacing3.Common.PrivateMessage;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

internal class PmOutgoingMessage : JsonOutgoingMessage<JsonPmOutgoingMessage>
{
	internal PmOutgoingMessage(IPrivateMessage pm) : base(new JsonPmOutgoingMessage(pm))
	{

	}
}