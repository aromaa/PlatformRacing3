using System.Text.Json.Serialization;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

internal sealed class JsonFinishDrawingOutgoingMessage : JsonPacket
{
	private protected override string InternalType => "finishDrawing";

	[JsonPropertyName("socketID")]
	public uint SocketId { get; set; }

	internal JsonFinishDrawingOutgoingMessage(uint socketId)
	{
		this.SocketId = socketId;
	}
}