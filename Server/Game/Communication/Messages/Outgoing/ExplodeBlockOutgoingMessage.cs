using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json.Rooms;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
    internal class ExplodeBlockOutgoingMessage : JsonOutgoingMessage
    {
        internal ExplodeBlockOutgoingMessage(string roomName, int tileY, int tileX) : base(new JsonExplodeBlockMessage(roomName, tileY, tileX))
        {
        }
    }
}
