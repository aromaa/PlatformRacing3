using System.Text.Json.Serialization;
using Platform_Racing_3_Common.PrivateMessage;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonPmsOutgoingMessage : JsonPacket
    {
        public override string Type => "receivePMs";

        [JsonPropertyName("requestID")]
        public uint RequestId { get; set; }

        [JsonPropertyName("results")]
        public uint Results { get; set; }

        [JsonPropertyName("pmArray")]
        public IReadOnlyCollection<IPrivateMessage> PMs { get; set; }

        internal JsonPmsOutgoingMessage(uint requestId, uint results, IReadOnlyCollection<IPrivateMessage> pms)
        {
            this.RequestId = requestId;
            this.Results = results;
            this.PMs = pms;
        }
    }
}
