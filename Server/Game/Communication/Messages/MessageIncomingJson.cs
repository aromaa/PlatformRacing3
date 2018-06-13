using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Net;

namespace Platform_Racing_3_Server.Game.Communication.Messages
{
    internal abstract class MessageIncomingJson<P> : IMessageIncomingJson where P: JsonPacket
    {
        public void Handle(ClientSession session, JsonPacket message)
        {
            this.Handle(session, (P)message);
        }

        internal abstract void Handle(ClientSession session, P message);
    }
}
