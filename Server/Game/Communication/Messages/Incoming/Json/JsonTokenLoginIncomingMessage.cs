using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal class JsonTokenLoginIncomingMessage : JsonPacket
    {
        [JsonProperty("login_token", Required = Required.Always)]
        internal string LoginToken { get; set; }
    }
}
