using System.Text.Json.Serialization;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonUserVarsOutgoingMessage : JsonPacket
    {
        public override string Type => "receiveUserVars";

        [JsonPropertyName("socketID")]
        public uint SocketId { get; set; }

        [JsonPropertyName("vars")]
        public IReadOnlyDictionary<string, object> Vars { get; set; }

        internal JsonUserVarsOutgoingMessage(uint socketId, IReadOnlyDictionary<string, object> vars)
        {
            this.SocketId = socketId;
            this.Vars = vars;
        }
    }
}
