using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Net.Buffers;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
    internal class JsonOutgoingMessage : IMessageOutgoing
    {
        private const ushort PACKET_HEADER = 0;

        private byte[] Json;

        internal JsonOutgoingMessage(JsonPacket jsonPacket)
        {
            this.Json = Encoding.UTF8.GetBytes(this.SerializeObject(jsonPacket));
        }

        protected virtual string SerializeObject(JsonPacket jsonPacket) => JsonConvert.SerializeObject(jsonPacket);

        public void Write(ref PacketWriter writer)
        {
            writer.WriteUInt16(JsonOutgoingMessage.PACKET_HEADER);
            writer.WriteBytes(this.Json);
        }
    }
}
