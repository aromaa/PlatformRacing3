using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Packets.Handshake
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
