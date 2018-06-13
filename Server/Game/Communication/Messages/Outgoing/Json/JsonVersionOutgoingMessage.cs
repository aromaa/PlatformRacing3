using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonVersionOutgoingMessage : JsonPacket
    {
        internal override string Type => "receiveVersion";

        [JsonProperty("version")]
        internal uint Version { get; set; }

        internal JsonVersionOutgoingMessage(uint version)
        {
            this.Version = version;
        }
    }
}
