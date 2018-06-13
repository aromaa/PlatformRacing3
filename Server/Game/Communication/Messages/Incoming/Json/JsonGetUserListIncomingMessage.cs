using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal class JsonGetUserListIncomingMessage : JsonPacket
    {
        [JsonProperty("list_type", Required = Required.Always)]
        internal string ListType { get; set; }

        [JsonProperty("request_id", Required = Required.Always)]
        internal uint RequestId { get; set; }

        [JsonProperty("start", Required = Required.Always)]
        internal uint Start { get; set; }

        [JsonProperty("Count", Required = Required.Always)]
        internal uint Count { get; set; }
    }
}
