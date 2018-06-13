using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class GetRoomsIncomingMessage : IMessageIncomingJson
    {
        public void Handle(ClientSession session, JsonPacket message)
        {
            session.SendPacket(new RoomsOutgoingMessage());
        }
    }
}
