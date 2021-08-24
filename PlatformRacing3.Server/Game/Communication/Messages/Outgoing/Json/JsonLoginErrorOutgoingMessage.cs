using System.Text.Json.Serialization;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
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
