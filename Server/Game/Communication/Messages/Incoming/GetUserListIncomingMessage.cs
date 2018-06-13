using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class GetUserListIncomingMessage : MessageIncomingJson<JsonGetUserListIncomingMessage>
    {
        internal override void Handle(ClientSession session, JsonGetUserListIncomingMessage message)
        {
            switch(message.ListType)
            {
                case "online":
                    {
                        session.SendPacket(new UserListOutgoingMessage(message.RequestId, PlatformRacing3Server.ClientManager.GetLoggedInUsers().Skip((int)message.Start).Take((int)message.Count).ToList().AsReadOnly()));
                    }
                    break;
            }
        }
    }
}
