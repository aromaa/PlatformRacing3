using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal sealed class JsonGetHatIncomingMessage : JsonPacket
    {
        [JsonPropertyName("id")]
        public uint Id { get; set; }
    }
}
