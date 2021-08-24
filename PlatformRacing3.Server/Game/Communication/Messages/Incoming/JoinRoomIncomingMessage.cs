using PlatformRacing3.Server.Game.Chat;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing;
using PlatformRacing3.Server.Game.Lobby;
using PlatformRacing3.Server.Game.Match;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming
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
