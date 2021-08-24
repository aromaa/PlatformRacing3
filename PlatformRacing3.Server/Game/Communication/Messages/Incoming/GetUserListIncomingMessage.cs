using System.Linq;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming
{
    internal sealed class GetUserListIncomingMessage : MessageIncomingJson<JsonGetUserListIncomingMessage>
    {
        private readonly ClientManager clientManager;

        public GetUserListIncomingMessage(ClientManager clientManager)
        {
            this.clientManager = clientManager;
        }

        internal override void Handle(ClientSession session, JsonGetUserListIncomingMessage message)
        {
            switch(message.ListType)
            {
                case "online":
                    {
                        session.SendPacket(new UserListOutgoingMessage(message.RequestId, this.clientManager.LoggedInUsers.Skip((int)message.Start).Take((int)message.Count).ToList().AsReadOnly(), (uint)this.clientManager.Count));
                    }
                    break;
            }
        }
    }
}
