using System.Text.Json.Serialization;
using PlatformRacing3.Common.PrivateMessage;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json
{
	internal sealed class JsonPmsOutgoingMessage : JsonPacket
    {
        private protected override string InternalType => "receivePMs";

        [JsonPropertyName("requestID")]
        public uint RequestId { get; set; }

        [JsonPropertyName("results")]
        public uint Results { get; set; }

        [JsonPropertyName("pmArray")]
        public IReadOnlyCollection<IPrivateMessage> PMs { get; set; }

        internal JsonPmsOutgoingMessage(uint requestId, uint results, IReadOnlyCollection<IPrivateMessage> pms)
        {
            this.RequestId = requestId;
            this.Results = results;
            this.PMs = pms;
        }
    }
}
