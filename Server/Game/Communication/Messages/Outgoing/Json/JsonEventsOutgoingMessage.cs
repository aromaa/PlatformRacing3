using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonEventsOutgoingMessage : JsonPacket
    {
        internal override string Type => "receiveEvents";

        [JsonProperty("events")]
        internal IReadOnlyCollection<string> Events { get; set; }

        internal JsonEventsOutgoingMessage(IReadOnlyCollection<string> events)
        {
            this.Events = events;
        }
    }
}
