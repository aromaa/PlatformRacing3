using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonVersionOutgoingMessage : JsonPacket
    {
        public override string Type => "receiveVersion";

        [JsonPropertyName("version")]
        public uint Version { get; set; }

        internal JsonVersionOutgoingMessage(uint version)
        {
            this.Version = version;
        }
    }
}
