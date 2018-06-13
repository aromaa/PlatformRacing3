using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonLogoutTriggerOutgoingMessage : JsonPacket
    {
        internal override string Type => "logoutTrigger";

        [JsonProperty("message")]
        internal string Message { get; set; }

        internal JsonLogoutTriggerOutgoingMessage(string message)
        {
            this.Message = message;
        }
    }
}
