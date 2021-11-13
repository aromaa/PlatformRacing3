using PlatformRacing3.Server.Game.Chat;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

internal class RoomsOutgoingMessage : JsonOutgoingMessage<JsonRoomsOutgoingMessage>
{
	internal RoomsOutgoingMessage(ICollection<ChatRoom> chatRooms) : base(new JsonRoomsOutgoingMessage(chatRooms))
	{
	}
}