using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Common.User;
using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class GetUserPageIncomingMessage : MessageIncomingJson<JsonGetUserPageIncomingMessage>
    {
        internal override void Handle(ClientSession session, JsonGetUserPageIncomingMessage message)
        {
            if (message.SocketId > 0 && PlatformRacing3Server.ClientManager.TryGetClientSessionBySocketId(message.SocketId, out ClientSession target))
            {
                this.SendUserPage(session, target.UserData, true);
            }
            else if (message.UserId > 0)
            {
                if (PlatformRacing3Server.ClientManager.TryGetClientSessionByUserId(message.UserId, out target))
                {
                    this.SendUserPage(session, target.UserData, true);
                }
                else
                {
                    UserManager.TryGetUserDataByIdAsync(message.UserId).ContinueWith((task) =>
                    {
                        if (task.Result != null)
                        {
                            this.SendUserPage(session, task.Result);
                        }
                        else
                        {
                            session.SendPacket(new AlertOutgoingMessage("Unable to load user data"));
                        }
                    });
                }
            }
            else
            {
                session.SendPacket(new AlertOutgoingMessage("Invalid user page request"));
            }
        }

        private void SendUserPage(ClientSession session, UserData userData, bool online = false)
        {
            ulong timestamp;
            if (online)
            {
                timestamp = (ulong)(userData.LastLogin.HasValue ? (DateTimeOffset.UtcNow - userData.LastLogin).Value.TotalMilliseconds : 0);
            }
            else
            {
                timestamp = (ulong)(userData.LastOnline.HasValue ? userData.LastOnline.Value.ToUnixTimeMilliseconds() : 0);
            }

            session.SendPacket(new UserPageOutgoingMessage(userData.Id, userData.Group, userData.Rank, online, timestamp, userData.CurrentHat, userData.CurrentHatColor, userData.CurrentHead, userData.CurrentHeadColor, userData.CurrentBody, userData.CurrentBodyColor, userData.CurrentFeet, userData.CurrentFeetColor));
        }
    }
}
