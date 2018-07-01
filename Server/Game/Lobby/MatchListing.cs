using Newtonsoft.Json;
using Platform_Racing_3_Common.Level;
using Platform_Racing_3_Common.User;
using Platform_Racing_3_Common.Utils;
using Platform_Racing_3_Server.Collections;
using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing;
using Platform_Racing_3_Server.Game.Match;
using Platform_Racing_3_Server.Game.User.Identifiers;
using Platform_Racing_3_Server_API.Net;
using Platform_Racing_3_UnsafeHelpers.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace Platform_Racing_3_Server.Game.Lobby
{
    internal class MatchListing
    {
        private const int SPOTS_LEFT_GAME_STARTED = int.MinValue;
        private const int SPOTS_LEFT_DIED = int.MinValue + 1;

        internal LevelData LevelData { get; }

        [JsonProperty("roomName")]
        internal string Name { get; }

        [JsonProperty("levelID")]
        internal uint LevelId => this.LevelData.Id;
        [JsonProperty("levelTitle")]
        internal string LevelTitle => this.LevelData.Title;

        [JsonProperty("creatorID")]
        internal uint CreatorId => this.LevelData.AuthorUserId;
        [JsonProperty("creatorName")]
        internal string CreatorName => this.LevelData.AuthorUsername;
        [JsonProperty("creatorNameColor")]
        internal Color CreatorNameColor => this.LevelData.AuthorNameColor;

        [JsonProperty("levelType")]
        internal LevelMode LevelMod => this.LevelData.Mode;

        [JsonProperty("likes")]
        public uint Likes => this.LevelData.Likes;
        [JsonProperty("dislikes")]
        public uint Dislikes => this.LevelData.Dislikes;

        [JsonProperty("minRank")]
        internal uint MinRank { get; }
        [JsonProperty("maxRank")]
        internal uint MaxRank { get; }

        [JsonProperty("maxMembers")]
        internal uint MaxMembers { get; }
        internal bool OnlyFriends { get; }

        private ClientSessionCollection _Clients;
        private ClientSessionCollection LobbyClients;
        private ConcurrentBag<IUserIdentifier> BannedClients;

        private uint _HostSocketId;
        public uint HostSocketId => this._HostSocketId;

        private int _SpotsLeft;
        public uint SpotsLeft => (uint)this._SpotsLeft;

        private volatile int UsersReady;

        internal MatchListing(ClientSession creator, LevelData levelData, string name, uint minRank, uint maxRank, uint maxMembers, bool onlyFriends)
        {
            this._Clients = new ClientSessionCollection(this.OnDisconnect);
            this.LobbyClients = new ClientSessionCollection();
            this.BannedClients = new ConcurrentBag<IUserIdentifier>();

            this.LevelData = levelData;

            this.Name = name;

            this.MinRank = minRank;
            this.MaxRank = maxRank;

            this.MaxMembers = maxMembers;
            this.OnlyFriends = onlyFriends;

            this._HostSocketId = creator.SocketId;
            this._SpotsLeft = (int)maxMembers;

            creator.OnDisconnect += this.OnCreatorDisconnectEarly;
            if (creator.Disconnected)
            {
                this.OnCreatorDisconnectEarly();
            }
        }

        internal ICollection<ClientSession> Clients => this._Clients.Values;
        internal uint ClientsCount => this._Clients.Count;

        private void OnDisconnect(ClientSession session)
        {
            this.Leave0(session, MatchListingLeaveReason.Disconnected);
        }

        private void OnCreatorDisconnectEarly(INetworkConnection networkConnection = null)
        {
            PlatformRacing3Server.MatchListingManager.Die(this); //We can pull the plug no biggie
        }

        internal MatchListingJoinStatus CanJoin(ClientSession session)
        {
            if (this._SpotsLeft <= 0) //No spots left, started or died
            {
                return this._SpotsLeft == MatchListing.SPOTS_LEFT_DIED ? MatchListingJoinStatus.Died : this._SpotsLeft == MatchListing.SPOTS_LEFT_GAME_STARTED ? MatchListingJoinStatus.Started : MatchListingJoinStatus.Full;
            }
            else if (this.HostSocketId == session.SocketId)
            {
                return MatchListingJoinStatus.Success;
            }
            else if (!this._Clients.Contains(this.HostSocketId))
            {
                return MatchListingJoinStatus.WaitingForHost;
            }
            else if ((this.MinRank > session.UserData.Rank || this.MaxRank < session.UserData.Rank) && !session.HasPermissions(Permissions.ACCESS_BYPASS_MATCH_LISTING_RANK_REQUIREMENT))
            {
                return MatchListingJoinStatus.NoRankRequirement;
            }
            else if (this.OnlyFriends && !session.HasPermissions(Permissions.ACCESS_BYPASS_MATCH_LISTING_ONLY_FRIENDS))
            {
                if (session.IsGuest)
                {
                    return MatchListingJoinStatus.FriendsOnly;
                }
                else if (this._Clients.TryGetClientSession(this.HostSocketId, out ClientSession host) && !host.IsGuest)
                {
                    return host.UserData.Friends.Contains(session.UserData.Id) ? MatchListingJoinStatus.Success : MatchListingJoinStatus.FriendsOnly;
                }
            }
            else if (this.BannedClients.Any((i) => i.Matches(session.UserData.Id, session.SocketId, session.IPAddres)))
            {
                return MatchListingJoinStatus.Banned;
            }

            return MatchListingJoinStatus.Success;
        }

        internal bool JoinLobby(ClientSession session)
        {
            if (this._SpotsLeft > 0)
            {
                this.LobbyClients.Add(session);
                if (this._SpotsLeft > 0)
                {
                    //Send the other clients
                    foreach (ClientSession other in this._Clients.Values)
                    {
                        session.TrackUserInRoom(this.Name, other.SocketId, other.UserData.Id, other.UserData.Username, other.GetVars("userName", "rank", "hat", "head", "body", "feet", "hatColor", "headColor", "bodyColor", "feetColor", "socketID", "ping"));
                    }

                    return true;
                }
            }

            return false;
        }

        internal void LeaveLobby(ClientSession session)
        {
            this.LobbyClients.Remove(session);

            session.UntrackUsersInRoom(this.Name);
        }

        internal MatchListingJoinStatus Join(ClientSession session)
        {
            if (!this._Clients.Contains(session))
            {
                MatchListingJoinStatus canJoinStatus = this.CanJoin(session);
                if (canJoinStatus != MatchListingJoinStatus.Success)
                {
                    return canJoinStatus;
                }

                while (true) //Concurrency complexity
                {
                    int spotsLeft = this._SpotsLeft;
                    if (spotsLeft > 0)
                    {
                        if (Interlocked.CompareExchange(ref this._SpotsLeft, spotsLeft - 1, spotsLeft) == spotsLeft)
                        {
                            break;
                        }
                    }
                    else
                    {
                        //We are assuming if spotsLeft is int.MinValue it means the match has been started or is in progress of starting
                        return spotsLeft == MatchListing.SPOTS_LEFT_GAME_STARTED ? MatchListingJoinStatus.Started : MatchListingJoinStatus.Full;
                    }
                }

                if (this._Clients.Add(session))
                {
                    session.OnDisconnect -= this.OnCreatorDisconnectEarly;

                    session.LobbySession.MatchListing = this;
                    session.LobbySession.RemoveMatch(this);

                    //Send the other clients
                    foreach (ClientSession other in this._Clients.Values.OrderBy((c) => c == session))
                    {
                        session.TrackUserInRoom(this.Name, other.SocketId, other.UserData.Id, other.UserData.Username, other.GetVars("userName", "rank", "hat", "head", "body", "feet", "hatColor", "headColor", "bodyColor", "feetColor", "socketID", "ping"));
                        other.TrackUserInRoom(this.Name, session.SocketId, session.UserData.Id, session.UserData.Username, session.GetVars("userName", "rank", "hat", "head", "body", "feet", "hatColor", "headColor", "bodyColor", "feetColor", "socketID", "ping"));
                    }

                    foreach (ClientSession other in this.LobbyClients.Values)
                    {
                        other.TrackUserInRoom(this.Name, session.SocketId, session.UserData.Id, session.UserData.Username, session.GetVars("userName", "rank", "hat", "head", "body", "feet", "hatColor", "headColor", "bodyColor", "feetColor", "socketID", "ping"));
                    }

                    uint currentHost = this._HostSocketId;
                    if (currentHost == session.SocketId || (currentHost == 0 && InterlockedExtansions.CompareExchange(ref this._HostSocketId, session.SocketId, currentHost) == currentHost))
                    {
                        session.SendPacket(new MatchOwnerOutgoingMessage(this.Name, true, true, true));
                    }
                    else if (!session.IsGuest)
                    {
                        bool play = session.HasPermissions(Permissions.ACCESS_FORCE_START_ANY_MATCH_LISTING);
                        bool kick = session.HasPermissions(Permissions.ACCESS_KICK_ANY_MATCH_LISTING);
                        bool ban = session.HasPermissions(Permissions.ACCESS_BAN_ANY_MATCH_LISTING);

                        if (play || kick || ban)
                        {
                            session.SendPacket(new MatchOwnerOutgoingMessage(this.Name, play, kick, ban));
                        }
                    }

                    //Concurrency complexity
                    if (Interlocked.Increment(ref this.UsersReady) == this.MaxMembers) //Ensures that there is no race condition while the SpotsLeft check is passed and user is being added to the clients list and ensuring safe start
                    {
                        if (Interlocked.CompareExchange(ref this._SpotsLeft, MatchListing.SPOTS_LEFT_GAME_STARTED, 0) == 0) //If no spots are left set the value to int.MinValue and start the match, starting should be thread-safe
                        {
                            this.Start();
                        }
                    }

                    return MatchListingJoinStatus.Success;
                }
                else
                {
                    while (true) //Concurrency complexity
                    {
                        int spotsLeft = this._SpotsLeft;
                        if (spotsLeft >= 0)
                        {
                            if (Interlocked.CompareExchange(ref this._SpotsLeft, spotsLeft + 1, spotsLeft) == spotsLeft)
                            {
                                break;
                            }
                        }
                    }

                    return MatchListingJoinStatus.Success; //We are gonna assume its failed due to dublication?
                }
            }
            else
            {
                return MatchListingJoinStatus.Success;
            }
        }

        internal void Leave(ClientSession session, MatchListingLeaveReason reason)
        {
            if (this._Clients.Remove(session))
            {
                this.Leave0(session, reason);
            }
        }

        internal void Leave0(ClientSession session, MatchListingLeaveReason reason)
        {
            session.LobbySession.MatchListing = null;

            //Concurrency complexity
            Interlocked.Decrement(ref this.UsersReady); //We can freely decrement this here without having issues

            while (true)
            {
                int spotsLeft = this._SpotsLeft;
                if (spotsLeft != MatchListing.SPOTS_LEFT_GAME_STARTED) //Match has not been started or it is in progress of starting
                {
                    if (Interlocked.CompareExchange(ref this._SpotsLeft, spotsLeft + 1, spotsLeft) == spotsLeft)
                    {
                        break; //Match has not been started
                    }
                }
            }

            foreach (ClientSession other in this._Clients.Values)
            {
                other.UntrackUserInRoom(this.Name, session.SocketId);
            }

            foreach (ClientSession other in this.LobbyClients.Values)
            {
                other.UntrackUserInRoom(this.Name, session.SocketId);
            }

            session.UntrackUsersInRoom(this.Name);

            if (reason == MatchListingLeaveReason.Kicked)
            {
                session.SendPacket(new UserLeaveRoomOutgoingMessage(this.Name, session.SocketId));
            }

            if (Interlocked.CompareExchange(ref this._SpotsLeft, (int)this.MaxMembers, MatchListing.SPOTS_LEFT_DIED) != (int)this.MaxMembers)
            {
                uint currentHost = this._HostSocketId;
                if (currentHost == session.SocketId) //Time to pick new host
                {
                    //TODO: How should we handle friends only?

                    while (this._Clients.Count > 0)
                    {
                        ClientSession other = this._Clients.Values.FirstOrDefault();
                        if (other != null)
                        {
                            if (InterlockedExtansions.CompareExchange(ref this._HostSocketId, other.SocketId, currentHost) == currentHost)
                            {
                                if (this._Clients.Contains(other))
                                {
                                    other.SendPacket(new MatchOwnerOutgoingMessage(this.Name, true, true, true));
                                    break;
                                }
                                else
                                {
                                    currentHost = other.SocketId;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (ClientSession other in this.LobbyClients.Values)
                {
                    other.LobbySession.RemoveMatch(this);
                }

                PlatformRacing3Server.MatchListingManager.Die(this);
            }
        }

        internal void ForceStart(ClientSession session)
        {
            if (this.HostSocketId == session.SocketId || session.HasPermissions(Permissions.ACCESS_FORCE_START_ANY_MATCH_LISTING))
            {
                //Concurrency adding a lot of complexity
                while (true)
                {
                    int spotsLeft = this._SpotsLeft;
                    if (spotsLeft != MatchListing.SPOTS_LEFT_GAME_STARTED)
                    {
                        if (Interlocked.CompareExchange(ref this._SpotsLeft, 0, spotsLeft) == spotsLeft) //Drain all spots, no new clients are accepted
                        {
                            if (this.UsersReady + spotsLeft != this.MaxMembers)
                            {
                                break; //The game will be started by the user that is doing join login
                            }

                            //We know there is no users trying to join, we can safely start the game
                            if (Interlocked.CompareExchange(ref this._SpotsLeft, 0, MatchListing.SPOTS_LEFT_GAME_STARTED) == 0)
                            {
                                this.Start();
                            }

                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        internal void Start()
        {
            PlatformRacing3Server.MatchListingManager.Die(this); //Kill us :(

            MultiplayerMatch match = PlatformRacing3Server.MatchManager.CreateMultiplayerMatch(this);

            StartGameOutgoingMessage startGame = new StartGameOutgoingMessage(this.Name, match.Name);
            foreach (ClientSession session in this._Clients.Values)
            {
                if (this._Clients.Remove(session))
                {
                    session.LobbySession.MatchListing = null;

                    if (match.Reserve(session))
                    {
                        session.SendPacket(startGame);
                    }
                }
            }

            match.Lock();

            foreach(ClientSession session in this.LobbyClients.Values)
            {
                if (this.LobbyClients.Remove(session))
                {
                    session.SendPacket(startGame);
                    session.UntrackUsersInRoom(this.Name);
                }
            }
        }

        internal void CheckState()
        {
            if (Interlocked.CompareExchange(ref this._SpotsLeft, (int)this.MaxMembers, MatchListing.SPOTS_LEFT_DIED) == (int)this.MaxMembers)
            {
                foreach (ClientSession other in this.LobbyClients.Values)
                {
                    other.LobbySession.RemoveMatch(this);
                }

                PlatformRacing3Server.MatchListingManager.Die(this);
            }
        }

        internal IReadOnlyDictionary<string, object> GetVars(params string[] vars) => JsonUtils.GetVars(this, vars);
        internal IReadOnlyDictionary<string, object> GetVars(HashSet<string> vars) => JsonUtils.GetVars(this, vars);

        internal void Kick(ClientSession session, uint socketId)
        {
            if (session.SocketId == this.HostSocketId || session.HasPermissions(Permissions.ACCESS_KICK_ANY_MATCH_LISTING))
            {
                if (this._Clients.TryGetClientSession(socketId, out ClientSession target))
                {
                    this.Leave(target, MatchListingLeaveReason.Kicked);
                }
            }
        }

        internal void Ban(ClientSession session, uint socketId)
        {
            if (session.SocketId == this.HostSocketId || session.HasPermissions(Permissions.ACCESS_BAN_ANY_MATCH_LISTING))
            {
                if (this._Clients.TryGetClientSession(socketId, out ClientSession target))
                {
                    if (target.IsGuest)
                    {
                        this.BannedClients.Add(new GuestIdentifier(target.SocketId, target.IPAddres));
                    }
                    else
                    {
                        this.BannedClients.Add(new PlayerIdentifier(target.UserData.Id));
                    }

                    this.Leave(target, MatchListingLeaveReason.Kicked);
                }
            }
        }
    }
}
