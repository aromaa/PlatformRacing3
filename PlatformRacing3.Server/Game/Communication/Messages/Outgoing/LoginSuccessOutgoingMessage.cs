using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

internal class LoginSuccessOutgoingMessage : JsonOutgoingMessage<JsonLoginSuccessOutgoingMessage>
{
	internal LoginSuccessOutgoingMessage(uint socketID, uint userID, string username, IReadOnlyCollection<string> permissions, IReadOnlyDictionary<string, object> vars) : base(new JsonLoginSuccessOutgoingMessage(socketID, userID, username, permissions, vars))
	{
	}
}