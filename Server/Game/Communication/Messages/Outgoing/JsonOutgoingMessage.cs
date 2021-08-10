using System.Text.Json.Serialization;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Net.Buffers;
using Platform_Racing_3_Common.Json;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
    internal class JsonOutgoingMessage<T> : IMessageOutgoing where T: JsonPacket
    {
	    private protected static readonly JsonSerializerOptions jsonSerializerOptions = new()
	    {
		    Converters =
		    {
			    new JsonColorConverter(),
			    new JsonLevelModeConverter(),
			    new JsonDateTimeConverter()
            }
	    };

        private const ushort PACKET_HEADER = 0;

        private byte[] Json;

        internal JsonOutgoingMessage(T jsonPacket)
        {
            this.Json = Encoding.UTF8.GetBytes(this.SerializeObject(jsonPacket));
        }

        private string SerializeObject(T jsonPacket) => JsonSerializer.Serialize(jsonPacket, JsonOutgoingMessage<T>.jsonSerializerOptions);

        public void Write(ref PacketWriter writer)
        {
            writer.WriteUInt16(JsonOutgoingMessage<T>.PACKET_HEADER);
            writer.WriteBytes(this.Json);
        }
    }
}
