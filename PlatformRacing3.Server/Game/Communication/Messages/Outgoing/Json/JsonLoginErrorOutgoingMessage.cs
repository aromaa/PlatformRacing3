using System.Text.Json.Serialization;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonLoginErrorOutgoingMessage : JsonPacket
    {
        private protected override string InternalType => "loginError";

        [JsonPropertyName("error")]
        public string Error { get; set; }

        internal JsonLoginErrorOutgoingMessage(string error)
        {
            this.Error = error;
        }
    }
}
