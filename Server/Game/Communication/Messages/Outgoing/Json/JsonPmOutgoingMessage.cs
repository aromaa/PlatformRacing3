using System.Text.Json.Serialization;
using Platform_Racing_3_Common.PrivateMessage;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonPmOutgoingMessage : JsonPacket
    {
        public override string Type => "receivePM";

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
