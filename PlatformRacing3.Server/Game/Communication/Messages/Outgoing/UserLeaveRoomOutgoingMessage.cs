using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

internal class UserLeaveRoomOutgoingMessage : JsonOutgoingMessage<JsonUserLeaveRoomOutgoingMessage>
{
	internal UserLeaveRoomOutgoingMessage(string roomName, uint socketId) : base(new JsonUserLeaveRoomOutgoingMessage(roomName, socketId))
	{
	}
}