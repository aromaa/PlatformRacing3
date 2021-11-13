using System.Text.Json.Serialization;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

internal sealed class JsonSocketIdOutgoingMessage : JsonPacket
{
	private protected override string InternalType => "receiveSocketID";

	[JsonPropertyName("socketID")]
	public uint SocketID { get; set; }

	internal JsonSocketIdOutgoingMessage(uint socketId)
	{
		this.SocketID = socketId;
	}
}