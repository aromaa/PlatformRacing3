using System.Text.Json.Serialization;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonUserListOutgoingMessage : JsonPacket
    {
        public override string Type => "receiveUserList";

        [JsonPropertyName("requestID")]
        public uint RequestId { get; set; }

        [JsonPropertyName("users")]
        public IReadOnlyCollection<IReadOnlyDictionary<string, object>> Users { get; set; }

        [JsonPropertyName("results")]
        public uint Results { get; private set; }

        internal JsonUserListOutgoingMessage(uint requestId, IReadOnlyCollection<ClientSession> sessions, uint total)
        {
            this.RequestId = requestId;

            List<IReadOnlyDictionary<string, object>> users = new();
            foreach(ClientSession session in sessions)
            {
                users.Add(session.GetVars("socketID", "userID", "userName", "rank", "hats", "status", "nameColor"));
            }

            this.Users = users;

            this.Results = total;
        }
    }
}
