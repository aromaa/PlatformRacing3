namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Packets.Handshake
{
    internal readonly struct RoundtripTimeIncomingPacket
    {
        internal readonly uint LastRoundtripTime;

        internal RoundtripTimeIncomingPacket(uint lastRoundtripTime)
        {
            this.LastRoundtripTime = lastRoundtripTime;
        }
    }
}
