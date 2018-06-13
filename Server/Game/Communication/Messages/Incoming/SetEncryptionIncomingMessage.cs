using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Net;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class SetEncryptionIncomingMessage : IMessageIncomingBytes
    {
        public void Handle(ClientSession session, IncomingMessage message)
        {
            throw new NotSupportedException();

            //uint seed = message.ReadUInt();

            //TODO: Implement this?
        }
    }
}
