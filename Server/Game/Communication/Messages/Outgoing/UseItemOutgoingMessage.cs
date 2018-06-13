using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json.Rooms;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
    internal class UseItemOutgoingMessage : JsonOutgoingMessage
    {
        internal UseItemOutgoingMessage(string roomName, uint socketId, double[] pos) : base(new JsonUseItemMessage(roomName, socketId, pos))
        {
        }
    }
}
