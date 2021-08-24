namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Packets.Handshake
{
    internal readonly struct SetEncryptionIncomingPacket
    {
        internal readonly uint Seed;

        internal SetEncryptionIncomingPacket(uint seed)
        {
            this.Seed = seed;
        }
    }
}
