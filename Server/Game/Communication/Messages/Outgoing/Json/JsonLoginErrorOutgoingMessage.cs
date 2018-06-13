using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonLoginErrorOutgoingMessage : JsonPacket
    {
        internal override string Type => "loginError";

        [JsonProperty("error")]
        internal string Error { get; set; }

        internal JsonLoginErrorOutgoingMessage(string error)
        {
            this.Error = error;
        }
    }
}
