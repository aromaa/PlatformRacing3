using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonRemoveHatOutgoingMessage : JsonPacket
    {
        internal override string Type => "removeHat";

        [JsonProperty("id")]
        internal uint Id { get; set; }

        internal JsonRemoveHatOutgoingMessage(uint id)
        {
            this.Id = id;
        }
    }
}
