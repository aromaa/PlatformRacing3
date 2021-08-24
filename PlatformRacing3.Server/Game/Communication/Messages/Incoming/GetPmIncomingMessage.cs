using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Common.PrivateMessage;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class GetPmIncomingMessage : MessageIncomingJson<JsonGetPmIncomingMessage>
    {
        internal override void Handle(ClientSession session, JsonGetPmIncomingMessage message)
        {
            if (session.IsGuest)
            {
                return;
            }

            IPrivateMessage privateMessage = PrivateMessageManager.GetPrivateMessageAsync(message.MessageId).Result;
            if (privateMessage?.ReceiverId == session.UserData.Id)
            {
                session.SendPacket(new PmOutgoingMessage(privateMessage));
            }
            else
            {
                session.SendPacket(new AlertOutgoingMessage("PM was not found!"));
            }
        }
    }
}
