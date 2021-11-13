using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

internal class UserListOutgoingMessage : JsonOutgoingMessage<JsonUserListOutgoingMessage>
{
	internal UserListOutgoingMessage(uint requestId, IReadOnlyCollection<ClientSession> sessions, uint total) : base(new JsonUserListOutgoingMessage(requestId, sessions, total))
	{
	}
}