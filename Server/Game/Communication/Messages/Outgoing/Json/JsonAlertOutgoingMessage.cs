using System.Text.Json.Serialization;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonAlertOutgoingMessage : JsonPacket
    {
        public override string Type => "alert";

        [JsonPropertyName("message")]
        public string Message { get; set; }

        internal JsonAlertOutgoingMessage(string message)
        {
            this.Message = message;
        }
    }
}
