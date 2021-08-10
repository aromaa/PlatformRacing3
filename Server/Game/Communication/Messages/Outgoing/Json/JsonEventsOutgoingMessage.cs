using System.Text.Json.Serialization;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonEventsOutgoingMessage : JsonPacket
    {
        public override string Type => "receiveEvents";

        [JsonPropertyName("events")]
        public IReadOnlyCollection<string> Events { get; set; }

        internal JsonEventsOutgoingMessage(IReadOnlyCollection<string> events)
        {
            this.Events = events;
        }
    }
}
