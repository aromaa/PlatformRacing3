using Platform_Racing_3_Common.Database;
using Platform_Racing_3_Common.PrivateMessage;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class GetPmsIncomingMessage : MessageIncomingJson<JsonGetPmsIncomingMessage>
    {
        internal override void Handle(ClientSession session, JsonGetPmsIncomingMessage message)
        {
            if (session.IsGuest)
            {
                return;
            }

            PrivateMessageManager.GetUserPMsAsync(session.UserData.Id, message.Start, message.Count).ContinueWith((task) =>
            {
                (uint Results, IReadOnlyList<IPrivateMessage> PMs) = task.Result;

                session.SendPacket(new PmsOutgoingMessage(message.RequestId, Results, PMs));
            });
        }
    }
}