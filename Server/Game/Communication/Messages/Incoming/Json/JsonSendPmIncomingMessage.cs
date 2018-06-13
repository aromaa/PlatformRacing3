using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Client;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal class JsonSendPmIncomingMessage : JsonPacket
    {
        [JsonProperty("name", Required = Required.Always)]
        internal string ReceiverUsername { get; set; }

        [JsonProperty("title", Required = Required.Always)]
        internal string Title { get; set; }

        [JsonProperty("message", Required = Required.Always)]
        internal string Message { get; set; }
    }
}
