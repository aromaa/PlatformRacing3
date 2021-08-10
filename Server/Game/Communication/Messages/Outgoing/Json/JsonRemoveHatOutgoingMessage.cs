using System.Text.Json.Serialization;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonRemoveHatOutgoingMessage : JsonPacket
    {
        public override string Type => "removeHat";

        [JsonPropertyName("id")]
        public uint Id { get; set; }

        internal JsonRemoveHatOutgoingMessage(uint id)
        {
            this.Id = id;
        }
    }
}
