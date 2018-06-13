using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonAlertOutgoingMessage : JsonPacket
    {
        internal override string Type => "alert";

        [JsonProperty("message")]
        internal string Message { get; set; }

        internal JsonAlertOutgoingMessage(string message)
        {
            this.Message = message;
        }
    }
}
