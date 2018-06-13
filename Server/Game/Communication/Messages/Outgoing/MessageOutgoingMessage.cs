using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
    internal class MessageOutgoingMessage : JsonOutgoingMessage
    {
        internal MessageOutgoingMessage(string roomName, JsonMessageOutgoingMessage.RoomMessageData data) : base(new JsonMessageOutgoingMessage(roomName, data))
        {
        }

        internal MessageOutgoingMessage(uint socketId, JsonMessageOutgoingMessage.RoomMessageData data) : base(new JsonMessageOutgoingMessage(socketId, data))
        {
        }

        internal MessageOutgoingMessage(string roomName, uint socketId, JsonMessageOutgoingMessage.RoomMessageData data) : base(new JsonMessageOutgoingMessage(roomName, socketId, data))
        {
        }
    }
}
