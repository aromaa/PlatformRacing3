using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Game.Lobby;
using Platform_Racing_3_Server.Game.Match;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal sealed class LeaveRoomIncomingMessage : MessageIncomingJson<JsonLeaveRoomIncomingMessage>
    {
        private readonly MatchListingManager matchListingManager;
        private readonly MatchManager matchManager;

        public LeaveRoomIncomingMessage(MatchListingManager matchListingManager, MatchManager matchManager)
        {
            this.matchListingManager = matchListingManager;
            this.matchManager = matchManager;
        }

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
                        this.matchListingManager.Leave(session, message.RoomName);
                    }
                    break;
                case "game":
                    {
                        this.matchManager.Leave(session, message.RoomName);
                    }
                    break;
            }
        }
    }
}
