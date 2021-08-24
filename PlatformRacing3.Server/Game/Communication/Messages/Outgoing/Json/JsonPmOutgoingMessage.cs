using System.Text.Json.Serialization;
using PlatformRacing3.Common.PrivateMessage;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonPmOutgoingMessage : JsonPacket
    {
        private protected override string InternalType => "receivePM";

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("senderName")]
        public string SenderUsername { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("allowHTML")]
        public bool AllowHTML { get; set; }

        [JsonPropertyName("handleAsJson")]
        public bool HandleAsJson { get; set; }

        internal JsonPmOutgoingMessage(IPrivateMessage pm)
        {
            this.Title = pm.Title;
            this.SenderUsername = pm.SenderUsername;
            this.Message = pm.Message;

            if (pm is TextPrivateMessage textPm)
            {
                this.AllowHTML = textPm.AllowHtml;
                this.HandleAsJson = false;
            }
            else if (pm is ThingTransferPrivateMessage thingTransferPm)
            {
                this.HandleAsJson = true;
            }
        }
    }
}
