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

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class JoinRoomIncomingMessage : MessageIncomingJson<JsonJoinRoomIncomingMessage>
    {
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
                        PlatformRacing3Server.ChatRoomManager.JoinOrCreate(session, message.RoomName, message.Pass, message.Note, out ChatRoomJoinStatus status);
                    }
                    break;
                case "match_listing":
                    {
                        MatchListing listing = PlatformRacing3Server.MatchListingManager.Join(session, message.RoomName, out MatchListingJoinStatus status);
                        if (listing == null)
                        {
                            session.SendPackets(new UserJoinRoomOutgoingMessage(message.RoomName, session.SocketId, session.UserData.Id, session.UserData.Username, session.GetVars("userName", "rank", "hat", "head", "body", "feet", "hatColor", "headColor", "bodyColor", "feetColor", "socketID", "ping")), new UserLeaveRoomOutgoingMessage(message.RoomName, session.SocketId));
                        }
                    }
                    break;
                case "game":
                    {
                        PlatformRacing3Server.MatchManager.JoinMultiplayerMatch(session, message.RoomName);
                    }
                    break;
            }
        }
    }
}
