using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal class JsonEditUserListIncomingMessage : JsonPacket
    {
        [JsonProperty("user_id", Required = Required.Always)]
        internal uint UserId { get; set; }

        [JsonProperty("list_type", Required = Required.Always)]
        internal string ListType { get; set; }
        
        [JsonProperty("action", Required = Required.Always)]
        internal string Action { get; set; }
    }
}
