using Net.Communication.Incoming.Helpers;
using Platform_Racing_3_Server.Game.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages
{
    internal interface IMessageIncomingBytes
    {
        void Handle(ClientSession session, ref PacketReader reader);
    }
}
