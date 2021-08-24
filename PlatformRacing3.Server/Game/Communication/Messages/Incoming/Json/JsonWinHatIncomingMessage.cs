using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal sealed class JsonWinHatIncomingMessage : JsonPacket
    {
        [JsonPropertyName("season")]
        public string Season { get; set; }

        [JsonPropertyName("medals")]
        public uint Medals { get; set; }
    }
}
