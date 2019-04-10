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
                        PlatformRacing3Server.ChatRoomManager.JoinOrCreate(session, message.RoomName, message.Pass, message.Note, out ChatRoomJoinStatus status, message.ChatId);
                    }
                    break;
                case "match_listing":
                    {
                        MatchListing listing = PlatformRacing3Server.MatchListingManager.Join(session, message.RoomName, out MatchListingJoinStatus status);
                        if (listing == null)
                        {
                            session.SendPackets(new UserJoinRoomOutgoingMessage(message.RoomName, session.SocketId, session.UserData.Id, session.UserData.Username, session.GetVars("userName", "rank", "hat", "head", "body", "feet", "hatColor", "headColor", "bodyColor", "feetColor", "socketID", "ping")), new UserLeaveRoomOutgoingMessage(message.RoomName, session.SocketId));

                            switch (status)
                            {
                                case MatchListingJoinStatus.Failed:
                                    {
                                        session.SendPacket(new AlertOutgoingMessage("Failed to join the match listing due to unknown error, sorry"));
                                        break;
                                    }
                                case MatchListingJoinStatus.WaitingForHost:
                                    {
                                        session.SendPacket(new AlertOutgoingMessage("You are trying to join the match too early, chill"));
                                        break;
                                    }
                                case MatchListingJoinStatus.Banned:
                                    {
                                        session.SendPacket(new AlertOutgoingMessage("You have been banned from this match listing"));
                                        break;
                                    }
                                case MatchListingJoinStatus.NoRankRequirement:
                                    {
                                        session.SendPacket(new AlertOutgoingMessage($"You don't meet the rank requirement, your rank must be at least {listing.MinRank} and not higer than {listing.MaxRank}"));
                                        break;
                                    }
                                case MatchListingJoinStatus.FriendsOnly:
                                    {
                                        session.SendPacket(new AlertOutgoingMessage($"You may only join if you are on the hosts friends list"));
                                        break;
                                    }
                                case MatchListingJoinStatus.Full:
                                    {
                                        session.SendPacket(new AlertOutgoingMessage($"The match listing is full"));
                                        break;
                                    }
                                case MatchListingJoinStatus.Started:
                                    {
                                        session.SendPacket(new AlertOutgoingMessage($"The match listing has been already started"));
                                        break;
                                    }
                                case MatchListingJoinStatus.Died:
                                    {
                                        session.SendPacket(new AlertOutgoingMessage($"The match listing has been closed due to everyone leaving"));
                                        break;
                                    }
                            }
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
