using System.Collections.Generic;
using System.Text.Json.Serialization;
using PlatformRacing3.Server.Game.Chat;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonRoomsOutgoingMessage : JsonPacket
    {
        private protected override string InternalType => "receiveRooms";

        [JsonPropertyName("roomList")]
        public IReadOnlyCollection<IReadOnlyDictionary<string, object>> Rooms { get; set; }

        internal JsonRoomsOutgoingMessage(ICollection<ChatRoom> chatRooms)
        {
            List<IReadOnlyDictionary<string, object>> rooms = new();
            foreach(ChatRoom chatRoom in chatRooms)
            {
                rooms.Add(chatRoom.GetVars("roomName", "members"));
            }

            this.Rooms = rooms;
        }
    }
}
