using System.Text.Json.Serialization;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonLogoutTriggerOutgoingMessage : JsonPacket
    {
        private protected override string InternalType => "logoutTrigger";

        [JsonPropertyName("message")]
        public string Message { get; set; }

        internal JsonLogoutTriggerOutgoingMessage(string message)
        {
            this.Message = message;
        }
    }
}
