using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal class JsonManageVarsIncomingMessage : JsonPacket
    {
        [JsonProperty("user_vars", Required = Required.Always)]
        internal HashSet<string> UserVars { get; set; }

        [JsonProperty("location", Required = Required.Always)]
        internal string Location { get; set; }

        [JsonProperty("action", Required = Required.Always)]
        internal string Action { get; set; }

        [JsonProperty("id", Required = Required.Always)]
        internal string Id { get; set; }
    }
}
