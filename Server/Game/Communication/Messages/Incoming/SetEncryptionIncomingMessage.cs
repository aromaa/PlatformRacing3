using System;
using System.Collections.Generic;
using System.Text;
using Net.Communication.Incoming.Helpers;
using Platform_Racing_3_Server.Game.Client;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class SetEncryptionIncomingMessage : IMessageIncomingBytes
    {
        public void Handle(ClientSession session, ref PacketReader reader)
        {
            throw new NotSupportedException();

            //uint seed = message.ReadUInt();

            //TODO: Implement this?
        }
    }
}
