using Platform_Racing_3_Common.PrivateMessage;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class ReportPmIncomingMessage : MessageIncomingJson<JsonReportPmIncomingMessage>
    {
        internal override void Handle(ClientSession session, JsonReportPmIncomingMessage message)
        {
            if (session.IsGuest)
            {
                return;
            }

            PrivateMessageManager.ReportPrivateMessageAsync(session.UserData.Id, message.MessageId);
        }
    }
}
