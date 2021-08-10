using System.Text.Json.Serialization;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonLegacyPingOutgoingMessage : JsonPacket
    {
        public override string Type => "ping";

        [JsonPropertyName("time")]
        public ulong Time { get; set; }

        [JsonPropertyName("server_time")]
        public ulong ServerTime { get; set; }

        internal JsonLegacyPingOutgoingMessage(ulong time, ulong serverTime)
        {
            this.Time = time;
            this.ServerTime = serverTime;
        }
    }
}
