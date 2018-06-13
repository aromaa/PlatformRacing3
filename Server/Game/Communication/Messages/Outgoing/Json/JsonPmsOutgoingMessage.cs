using Newtonsoft.Json;
using Platform_Racing_3_Common.PrivateMessage;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonPmsOutgoingMessage : JsonPacket
    {
        internal override string Type => "receivePMs";

        [JsonProperty("requestID")]
        internal uint RequestId { get; set; }

        [JsonProperty("results")]
        internal uint Results { get; set; }

        [JsonProperty("pmArray")]
        internal IReadOnlyCollection<IPrivateMessage> PMs { get; set; }

        internal JsonPmsOutgoingMessage(uint requestId, uint results, IReadOnlyCollection<IPrivateMessage> pms)
        {
            this.RequestId = requestId;
            this.Results = results;
            this.PMs = pms;
        }
    }
}
