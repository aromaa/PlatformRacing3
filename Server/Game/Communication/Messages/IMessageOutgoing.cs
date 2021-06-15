using System;
using System.Collections.Generic;
using System.Text;
using Net.Buffers;

namespace Platform_Racing_3_Server.Game.Communication.Messages
{
    internal interface IMessageOutgoing
    {
        void Write(ref PacketWriter writer);
    }
}
