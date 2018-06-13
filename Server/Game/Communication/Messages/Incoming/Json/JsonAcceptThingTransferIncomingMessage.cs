using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal class JsonAcceptThingTransferIncomingMessage : JsonPacket
    {
        [JsonProperty("transfer_id", Required = Required.Always)]
        internal uint TransferId { get; set; }

        [JsonProperty("title", Required = Required.Always)]
        internal string Title { get; set; }

        [JsonProperty("comment", Required = Required.Always)]
        internal string Description { get; set; }

        [JsonProperty("category")]
        internal string Category { get; set; }

        [JsonProperty("publish", Required = Required.Always)]
        internal bool Publish { get; set; }
    }
}
