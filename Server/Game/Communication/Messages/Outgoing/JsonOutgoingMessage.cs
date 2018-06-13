using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
    internal class JsonOutgoingMessage : IMessageOutgoing
    {
        private const ushort PACKET_HEADER = 0;

        private byte[] Bytes { get; }

        internal JsonOutgoingMessage(JsonPacket jsonPacket)
        {
            ServerMessage message = new ServerMessage();
            message.WriteUShort(JsonOutgoingMessage.PACKET_HEADER);
            message.WriteBytes(Encoding.UTF8.GetBytes(this.SerializeObject(jsonPacket)));
            this.Bytes = message.GetBytes();
        }

        protected virtual string SerializeObject(JsonPacket jsonPacket) => JsonConvert.SerializeObject(jsonPacket);

        public byte[] GetBytes() => this.Bytes;
    }
}
