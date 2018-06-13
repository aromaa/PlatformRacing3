using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonUserVarsOutgoingMessage : JsonPacket
    {
        internal override string Type => "receiveUserVars";

        [JsonProperty("socketID")]
        internal uint SocketId { get; set; }

        [JsonProperty("vars")]
        internal IReadOnlyDictionary<string, object> Vars { get; set; }

        internal JsonUserVarsOutgoingMessage(uint socketId, IReadOnlyDictionary<string, object> vars)
        {
            this.SocketId = socketId;
            this.Vars = vars;
        }
    }
}
