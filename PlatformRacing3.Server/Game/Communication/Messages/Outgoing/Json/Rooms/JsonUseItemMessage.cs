using System.Text.Json.Serialization;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json.Rooms;

internal sealed class JsonUseItemMessage : JsonMessageOutgoingMessage
{
	internal JsonUseItemMessage(string roomName, uint socketId, double[] pos) : base(roomName, socketId, new RoomMessageData("useItem", new UseItemData(pos)))
	{
	}

	private sealed class UseItemData
	{
		[JsonPropertyName("p")]
		public double[] Pos { get; set; }

		internal UseItemData(double[] pos)
		{
			this.Pos = pos;
		}
	}
}