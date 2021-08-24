using System.Text.Json.Serialization;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Net.Buffers;
using Platform_Racing_3_Common.Json;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
    internal abstract class JsonOutgoingMessage<T> : IMessageOutgoing where T: JsonPacket
    {
        private static readonly JsonOutgoingPacketContext jsonContext = new(new JsonSerializerOptions
        {
            Converters =
            {
	            new JsonColorConverter(),
	            new JsonLevelModeConverter(),
	            new JsonDateTimeConverter()
            }
        });

        private static readonly JsonTypeInfo<T> jsonTypeInfo = (JsonTypeInfo<T>)JsonOutgoingMessage<T>.jsonContext.GetTypeInfo(typeof(T));

        private const ushort PACKET_HEADER = 0;

        private byte[] Json;

        internal JsonOutgoingMessage(T jsonPacket)
        {
            this.Json = JsonSerializer.SerializeToUtf8Bytes(jsonPacket, JsonOutgoingMessage<T>.jsonContext.Options);
        }
        
        public void Write(ref PacketWriter writer)
        {
            writer.WriteUInt16(JsonOutgoingMessage<T>.PACKET_HEADER);
            writer.WriteBytes(this.Json);
        }
    }
}
