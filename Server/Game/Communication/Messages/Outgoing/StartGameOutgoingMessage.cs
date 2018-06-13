using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
    internal class StartGameOutgoingMessage : JsonOutgoingMessage
    {
        internal StartGameOutgoingMessage(string roomName, string gameName) : base(new JsonStartGameMessage(roomName, gameName))
        {
        }
    }
}
