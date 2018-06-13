using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Game.Client;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class RoundtripTimeIncomingMessage : IMessageIncomingBytes
    {
        public void Handle(ClientSession session, IncomingMessage message)
        {
            session.LastRoundtripTime = message.ReadUInt();
        }
    }
}
