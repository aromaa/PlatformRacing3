using System.Text.Json.Serialization;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
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
