using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Chat;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class GetMemberListIncomingMessage : MessageIncomingJson<JsonGetMemberListIncomingMessage>
    {
        internal override void Handle(ClientSession session, JsonGetMemberListIncomingMessage message)
        {
            if (!session.IsLoggedIn)
            {
                return;
            }

            if (PlatformRacing3Server.ChatRoomManager.TryGet(message.RoomName, out ChatRoom chatRoom))
            {
                session.SendPacket(new MemberListOutgoingMessage(chatRoom.Members));
            }
        }
    }
}
