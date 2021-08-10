using System.Text.Json.Serialization;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonTournamentStatusOutgoingMessage : JsonPacket
    {
        public override string Type => "tournament_status";

        [JsonPropertyName("status")]
        public byte Status { get; set; }

        internal JsonTournamentStatusOutgoingMessage(byte status)
        {
            this.Status = status;
        }
    }
}
