using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal sealed class JsonManageVarsIncomingMessage : JsonPacket
    {
        [JsonPropertyName("user_vars")]
        public HashSet<string> UserVars { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("action")]
        public string Action { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
