using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal sealed class JsonEditUserListIncomingMessage : JsonPacket
    {
        [JsonPropertyName("user_id")]
        public uint UserId { get; set; }

        [JsonPropertyName("list_type")]
        public string ListType { get; set; }
        
        [JsonPropertyName("action")]
        public string Action { get; set; }
    }
}
