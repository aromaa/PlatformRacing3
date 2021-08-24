using PlatformRacing3.Common.PrivateMessage;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming
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
