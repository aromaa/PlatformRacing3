using Net.Buffers;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing
{
    internal class PingOutgoingMessage : IMessageOutgoing
    {
        private const ushort PACKET_ID = 44;

        internal static PingOutgoingMessage Instance { get; } = new PingOutgoingMessage();

        public void Write(ref PacketWriter writer)
        {
            writer.WriteUInt16(PingOutgoingMessage.PACKET_ID);
        }
    }
}
