using Newtonsoft.Json;
using Platform_Racing_3_Common.Level;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonLevelListOutgoingMessage : JsonPacket
    {
        internal override string Type => "receiveLevelList";

        [JsonProperty("requestID")]
        internal uint RequestId { get; set; }

        [JsonProperty("levels")]
        internal IReadOnlyCollection<LevelData> Levels { get; set; }

        [JsonProperty("results")]
        internal uint Results { get; set; }

        internal JsonLevelListOutgoingMessage(uint requestId, uint results, IReadOnlyCollection<LevelData> levels)
        {
            this.RequestId = requestId;
            this.Results = results;
            this.Levels = levels;
        }
    }
}
