using PlatformRacing3.Common.PrivateMessage;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

internal class PmsOutgoingMessage : JsonOutgoingMessage<JsonPmsOutgoingMessage>
{
	internal PmsOutgoingMessage(uint requestId, uint results, IReadOnlyCollection<IPrivateMessage> pms) : base(new JsonPmsOutgoingMessage(requestId, results, pms))
	{
	}
}