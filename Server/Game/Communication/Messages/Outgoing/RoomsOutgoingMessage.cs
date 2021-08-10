using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Chat;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing
{
    internal class RoomsOutgoingMessage : JsonOutgoingMessage<JsonRoomsOutgoingMessage>
    {
        internal RoomsOutgoingMessage(ICollection<ChatRoom> chatRooms) : base(new JsonRoomsOutgoingMessage(chatRooms))
        {
        }
    }
}
