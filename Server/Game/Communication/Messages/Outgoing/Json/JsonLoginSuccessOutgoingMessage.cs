using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonLoginSuccessOutgoingMessage : JsonPacket
    {
        internal override string Type => "loginSuccess";

        [JsonProperty("socketID")]
        internal uint SocketID { get; set; }

        [JsonProperty("userID")]
        internal uint UserID { get; set; }

        [JsonProperty("userName")]
        internal string Username { get; set; }

        [JsonProperty("permissions")]
        internal IReadOnlyCollection<string> Permissions { get; set; }

        [JsonProperty("vars")]
        internal IReadOnlyDictionary<string, object> Vars { get; set; }

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
