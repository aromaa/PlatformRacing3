using System.Text.Json.Serialization;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json
{
	internal sealed class JsonMemberListOutgoingMessage : JsonPacket
    {
        private protected override string InternalType => "memberList";

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
