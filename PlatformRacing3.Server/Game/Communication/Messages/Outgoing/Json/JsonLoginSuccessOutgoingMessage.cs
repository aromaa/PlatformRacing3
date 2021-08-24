using System.Text.Json.Serialization;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonLoginSuccessOutgoingMessage : JsonPacket
    {
        private protected override string InternalType => "loginSuccess";

        [JsonPropertyName("socketID")]
        public uint SocketID { get; set; }

        [JsonPropertyName("userID")]
        public uint UserID { get; set; }

        [JsonPropertyName("userName")]
        public string Username { get; set; }

        [JsonPropertyName("permissions")]
        public IReadOnlyCollection<string> Permissions { get; set; }

        [JsonPropertyName("vars")]
        public IReadOnlyDictionary<string, object> Vars { get; set; }

        internal JsonLoginSuccessOutgoingMessage(uint socketID, uint userID, string username, IReadOnlyCollection<string> permissions, IReadOnlyDictionary<string, object> vars)
        {
            this.SocketID = socketID;
            this.UserID = userID;
            this.Username = username;
            this.Permissions = permissions;
            this.Vars = vars;
        }
    }
}
