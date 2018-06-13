using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonLegacyPingOutgoingMessage : JsonPacket
    {
        internal override string Type => "ping";

        [JsonProperty("time")]
        internal ulong Time { get; set; }

        [JsonProperty("server_time")]
        internal ulong ServerTime { get; set; }

        internal JsonLegacyPingOutgoingMessage(ulong time, ulong serverTime)
        {
            this.Time = time;
            this.ServerTime = serverTime;
        }
    }
}
