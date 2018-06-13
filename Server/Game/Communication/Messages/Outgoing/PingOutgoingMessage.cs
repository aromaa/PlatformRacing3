using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
    internal class PingOutgoingMessage : IMessageOutgoing
    {
        private const ushort PACKET_ID = 44;

        internal static PingOutgoingMessage Instance { get; } = new PingOutgoingMessage();

        private byte[] Bytes { get; }

        private PingOutgoingMessage()
        {
            ServerMessage message = new ServerMessage();
            message.WriteUShort(PingOutgoingMessage.PACKET_ID);
            this.Bytes = message.GetBytes();
        }

        public byte[] GetBytes() => this.Bytes;
    }
}
