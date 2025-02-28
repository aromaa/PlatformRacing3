using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json.Rooms;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

internal class StartGameOutgoingMessage : JsonOutgoingMessage<JsonStartGameMessage>
{
	internal StartGameOutgoingMessage(string roomName, string gameName, int random) : base(new JsonStartGameMessage(roomName, gameName, random))
	{
	}
}