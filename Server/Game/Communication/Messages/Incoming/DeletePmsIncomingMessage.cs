using Platform_Racing_3_Common.PrivateMessage;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class DeletePmsIncomingMessage : MessageIncomingJson<JsonDeletePmsIncomingMessage>
    {
        internal override void Handle(ClientSession session, JsonDeletePmsIncomingMessage message)
        {
            if (session.IsGuest)
            {
                return;
            }

            PrivateMessageManager.DeletePMsAsync(message.PMs, session.UserData.Id).Wait();
        }
    }
}
