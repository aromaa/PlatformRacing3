using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Lobby;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonReceiveLevelOfTheDayOutgoingMessage : JsonPacket
    {
        internal override string Type => "receiveLOTD";

        [JsonProperty("lotd")]
        public IReadOnlyDictionary<string, object> Lotd { get; set; }

        internal JsonReceiveLevelOfTheDayOutgoingMessage(MatchListing matchListing)
        {
            this.Lotd = matchListing.GetVars("*");
        }
    }
}
