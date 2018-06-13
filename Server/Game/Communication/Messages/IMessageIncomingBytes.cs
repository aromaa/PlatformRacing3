using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages
{
    internal interface IMessageIncomingBytes
    {
        void Handle(ClientSession session, IncomingMessage message);
    }
}
