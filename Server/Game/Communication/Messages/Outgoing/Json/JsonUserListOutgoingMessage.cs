using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonUserListOutgoingMessage : JsonPacket
    {
        internal override string Type => "receiveUserList";

        [JsonProperty("requestID")]
        internal uint RequestId { get; set; }

        [JsonProperty("users")]
        internal IReadOnlyCollection<IReadOnlyDictionary<string, object>> Users { get; set; }

        [JsonProperty("results")]
        internal uint Results { get; private set; }

        internal JsonUserListOutgoingMessage(uint requestId, IReadOnlyCollection<ClientSession> sessions, uint total)
        {
            this.RequestId = requestId;

            List<IReadOnlyDictionary<string, object>> users = new List<IReadOnlyDictionary<string, object>>();
            foreach(ClientSession session in sessions)
            {
                users.Add(session.GetVars("socketID", "userID", "userName", "rank", "hats", "status", "nameColor"));
            }

            this.Users = users;

            this.Results = total;
        }
    }
}
