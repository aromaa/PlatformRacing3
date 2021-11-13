using System.Text.Json.Serialization;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json
{
	internal sealed class JsonEventsOutgoingMessage : JsonPacket
    {
        private protected override string InternalType => "receiveEvents";

        [JsonPropertyName("events")]
        public IReadOnlyCollection<string> Events { get; set; }

        internal JsonEventsOutgoingMessage(IReadOnlyCollection<string> events)
        {
            this.Events = events;
        }
    }
}
