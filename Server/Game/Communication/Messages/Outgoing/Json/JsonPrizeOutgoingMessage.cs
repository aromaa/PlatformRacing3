using System.Text.Json.Serialization;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Match;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonPrizeOutgoingMessage : JsonPacket
    {
        public override string Type => "prize";

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("id")]
        public uint Id { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        internal JsonPrizeOutgoingMessage(MatchPrize prize, string status)
        {
            this.Category = prize.Category;
            this.Id = prize.Id;
            this.Status = status;
        }
    }
}
