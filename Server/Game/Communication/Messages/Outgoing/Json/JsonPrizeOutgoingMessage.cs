using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Match;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonPrizeOutgoingMessage : JsonPacket
    {
        internal override string Type => "prize";

        [JsonProperty("category")]
        internal string Category { get; set; }

        [JsonProperty("id")]
        internal uint Id { get; set; }

        [JsonProperty("status")]
        internal string Status { get; set; }

        internal JsonPrizeOutgoingMessage(MatchPrize prize, string status)
        {
            this.Category = prize.Category;
            this.Id = prize.Id;
            this.Status = status;
        }
    }
}
