using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal sealed class JsonGetPmIncomingMessage : JsonPacket
    {
        [JsonPropertyName("message_id")]
        public uint MessageId { get; set; }
    }
}
