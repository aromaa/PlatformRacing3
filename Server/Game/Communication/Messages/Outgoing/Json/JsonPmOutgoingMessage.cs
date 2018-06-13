using Newtonsoft.Json;
using Platform_Racing_3_Common.PrivateMessage;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonPmOutgoingMessage : JsonPacket
    {
        internal override string Type => "receivePM";

        [JsonProperty("title")]
        internal string Title { get; set; }

        [JsonProperty("senderName")]
        internal string SenderUsername { get; set; }

        [JsonProperty("message")]
        internal string Message { get; set; }

        [JsonProperty("allowHTML")]
        internal bool AllowHTML { get; set; }

        [JsonProperty("handleAsJson")]
        internal bool HandleAsJson { get; set; }

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
