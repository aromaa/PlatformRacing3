using Newtonsoft.Json;
using Platform_Racing_3_Common.Utils;
using Platform_Racing_3_Server.Game.Chat;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonRoomsOutgoingMessage : JsonPacket
    {
        internal override string Type => "receiveRooms";

        [JsonProperty("roomList")]
        internal IReadOnlyCollection<IReadOnlyDictionary<string, object>> Rooms { get; set; }

        internal JsonRoomsOutgoingMessage(ICollection<ChatRoom> chatRooms)
        {
            List<IReadOnlyDictionary<string, object>> rooms = new List<IReadOnlyDictionary<string, object>>();
            foreach(ChatRoom chatRoom in chatRooms)
            {
                rooms.Add(chatRoom.GetVars("roomName", "members"));
            }

            this.Rooms = rooms;
        }
    }
}
