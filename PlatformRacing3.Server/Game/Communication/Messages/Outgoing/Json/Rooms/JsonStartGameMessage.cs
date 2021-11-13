using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json.Rooms;

internal sealed class JsonStartGameMessage : JsonMessageOutgoingMessage
{
	internal JsonStartGameMessage(string roomName, string gameName) : base(roomName, new StartGameData(gameName))
	{
	}

	internal sealed class StartGameData : RoomMessageData
	{
		[JsonPropertyName("gameName")]
		public string GameName { get; set; }

		internal StartGameData(string gameName) : base("startGame", null)
		{
			this.GameName = gameName;
		}
	}
}