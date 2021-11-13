using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing
{
	internal class MemberListOutgoingMessage : JsonOutgoingMessage<JsonMemberListOutgoingMessage>
    {
        internal MemberListOutgoingMessage(ICollection<ClientSession> members) : base(new JsonMemberListOutgoingMessage(members))
        {
        }
    }
}
