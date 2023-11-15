using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Net.Buffers;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

internal abstract class JsonOutgoingMessage<T> : IMessageOutgoing where T: JsonPacket
{
	private static readonly JsonTypeInfo<T> jsonTypeInfo = (JsonTypeInfo<T>)JsonPacketContext.Default.GetTypeInfo(typeof(T));

	private const ushort PACKET_HEADER = 0;

	private byte[] Json;

	internal JsonOutgoingMessage(T jsonPacket)
	{
		this.Json = JsonSerializer.SerializeToUtf8Bytes(jsonPacket, JsonOutgoingMessage<T>.jsonTypeInfo);
	}
        
	public void Write(ref PacketWriter writer)
	{
		writer.WriteUInt16(JsonOutgoingMessage<T>.PACKET_HEADER);
		writer.WriteBytes(this.Json);
	}
}