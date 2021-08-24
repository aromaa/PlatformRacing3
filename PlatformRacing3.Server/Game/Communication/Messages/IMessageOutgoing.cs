using Net.Buffers;

namespace PlatformRacing3.Server.Game.Communication.Messages
{
    internal interface IMessageOutgoing
    {
        void Write(ref PacketWriter writer);
    }
}
