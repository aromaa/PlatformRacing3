using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class GetTournamentIncomingMessage : IMessageIncomingJson
    {
        public void Handle(ClientSession session, JsonPacket message)
        {
            //TODO: Do
        }
    }
}
