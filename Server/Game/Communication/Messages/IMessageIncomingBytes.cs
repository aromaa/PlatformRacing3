using Platform_Racing_3_Server.Game.Client;
using System;
using System.Collections.Generic;
using System.Text;
using Net.Buffers;

namespace Platform_Racing_3_Server.Game.Communication.Messages
{
    internal interface IMessageIncomingBytes
    {
        void Handle(ClientSession session, ref PacketReader reader);
    }
}
