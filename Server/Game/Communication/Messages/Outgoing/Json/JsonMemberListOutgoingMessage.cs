using System.Text.Json.Serialization;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonMemberListOutgoingMessage : JsonPacket
    {
        public override string Type => "memberList";

        [JsonPropertyName("list")]
        public IReadOnlyCollection<IReadOnlyDictionary<string, object>> Members { get; set; }

        internal JsonMemberListOutgoingMessage(ICollection<ClientSession> members)
        {
            List<IReadOnlyDictionary<string, object>> membersList = new();
            foreach(ClientSession member in members)
            {
                membersList.Add(member.GetVars("socketID", "userID", "userName", "nameColor"));
            }

            this.Members = membersList;
        }
    }
}
