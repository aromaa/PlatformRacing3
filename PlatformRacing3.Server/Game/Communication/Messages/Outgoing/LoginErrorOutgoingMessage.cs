using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

internal class LoginErrorOutgoingMessage : JsonOutgoingMessage<JsonLoginErrorOutgoingMessage>
{
	internal LoginErrorOutgoingMessage(string error) : base(new JsonLoginErrorOutgoingMessage(error))
	{
	}
}