using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json.Rooms;

internal sealed class JsonStartGameMessage : JsonMessageOutgoingMessage
{
	internal JsonStartGameMessage(string roomName, string gameName, int random) : base(roomName, new StartGameData(gameName, random))
	{
	}

	internal sealed class StartGameData : RoomMessageData
	{
		[JsonPropertyName("gameName")]
		public string GameName { get; set; }

		[JsonPropertyName("random")]
		public int Random { get; set; }

		internal StartGameData(string gameName, int random) : base("startGame", null)
		{
			this.GameName = gameName;
			this.Random = random;
		}
	}
}