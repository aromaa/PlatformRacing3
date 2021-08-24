using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Platform_Racing_3_Server.Game.Client;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal sealed class JsonSendPmIncomingMessage : JsonPacket
    {
        [JsonPropertyName("name")]
        public string ReceiverUsername { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
