using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class LeaveRoomIncomingMessage : MessageIncomingJson<JsonLeaveRoomIncomingMessage>
    {
        internal override void Handle(ClientSession session, JsonLeaveRoomIncomingMessage message)
        {
            if (!session.IsLoggedIn)
            {
                return;
            }

            switch (message.RoomType)
            {
                case "chat":
                    {
                        PlatformRacing3Server.ChatRoomManager.Leave(session, message.RoomName);
                    }
                    break;
                case "match_listing":
                    {
                        PlatformRacing3Server.MatchListingManager.Leave(session, message.RoomName);
                    }
                    break;
                case "game":
                    {
                        PlatformRacing3Server.MatchManager.Leave(session, message.RoomName);
                    }
                    break;
            }
        }
    }
}
