using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonTournamentStatusOutgoingMessage : JsonPacket
    {
        internal override string Type => "tournament_status";

        [JsonProperty("status")]
        internal byte Status { get; set; }

        internal JsonTournamentStatusOutgoingMessage(byte status)
        {
            this.Status = status;
        }
    }
}
