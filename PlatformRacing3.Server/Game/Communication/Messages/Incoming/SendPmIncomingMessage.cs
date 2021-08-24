using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Common.PrivateMessage;
using Platform_Racing_3_Common.User;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class SendPmIncomingMessage : MessageIncomingJson<JsonSendPmIncomingMessage>
    {
        internal override void Handle(ClientSession session, JsonSendPmIncomingMessage message)
        {
            if (session.IsGuest)
            {
                return;
            }

            if (message.Message.Length > 0)
            {
                UserManager.TryGetUserDataByNameAsync(message.ReceiverUsername).ContinueWith((task) =>
                {
                    PlayerUserData userData = task.Result;
                    if (userData != null)
                    {
                        PrivateMessageManager.SendTextPrivateMessageAsync(userData.Id, session.UserData.Id, message.Title, message.Message);
                    }
                    else
                    {
                        session.SendPacket(new AlertOutgoingMessage("User was not found!"));
                    }
                });
            }
            else
            {
                session.SendPacket(new AlertOutgoingMessage("What if you typed message before sending?"));
            }
        }
    }
}
