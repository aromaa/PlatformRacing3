using Platform_Racing_3_Common.Utils;
using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Chat;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing;
using Platform_Racing_3_Server.Game.Lobby;
using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Server.Game.Match;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal sealed class JoinRoomIncomingMessage : MessageIncomingJson<JsonJoinRoomIncomingMessage>
    {
        private readonly ChatRoomManager chatRoomManager;

        private readonly MatchListingManager matchListingManager;
        private readonly MatchManager matchManager;

        public JoinRoomIncomingMessage(ChatRoomManager chatRoomManager, MatchListingManager matchListingManager, MatchManager matchManager)
        {
            this.chatRoomManager = chatRoomManager;

            this.matchListingManager = matchListingManager;
            this.matchManager = matchManager;
        }

        internal override void Handle(ClientSession session, JsonJoinRoomIncomingMessage message)
        {
            if (!session.IsLoggedIn)
            {
                return;
            }

            switch (message.RoomType)
            {
                case "chat":
                    {
                        this.chatRoomManager.JoinOrCreate(session, message.RoomName, message.Pass, message.Note, out bool status, message.ChatId);
                    }
                    break;
                case "match_listing":
                    {
                        MatchListing listing = this.matchListingManager.Join(session, message.RoomName, out bool status);
                        if (listing == null)
                        {
                            session.SendPacket(new UserJoinRoomOutgoingMessage(message.RoomName, session.SocketId, session.UserData.Id, session.UserData.Username, session.GetVars("userName", "rank", "hat", "head", "body", "feet", "hatColor", "headColor", "bodyColor", "feetColor", "socketID", "ping")));
                            session.SendPacket(new UserLeaveRoomOutgoingMessage(message.RoomName, session.SocketId));

                            if (!status)
                            {
                                session.SendPacket(new AlertOutgoingMessage("Failed to join the match listing!"));
                            }
                        }
                    }
                    break;
                case "game":
                    {
                        this.matchManager.JoinMultiplayerMatch(session, message.RoomName);
                    }
                    break;
            }
        }
    }
}
