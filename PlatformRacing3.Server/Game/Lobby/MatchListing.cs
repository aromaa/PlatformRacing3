using System.Collections.Concurrent;
using System.Drawing;
using System.Text.Json.Serialization;
using Net.Sockets;
using PlatformRacing3.Common.Level;
using PlatformRacing3.Common.User;
using PlatformRacing3.Common.Utils;
using PlatformRacing3.Server.Collections;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing;
using PlatformRacing3.Server.Game.Match;
using PlatformRacing3.Server.Game.User.Identifiers;

namespace PlatformRacing3.Server.Game.Lobby
{
	internal class MatchListing
    {
        private const int SPOTS_LEFT_GAME_STARTED = int.MinValue;
        private const int SPOTS_LEFT_DIED = int.MinValue + 1;

        private readonly MatchListingManager matchListingManager;
        private readonly MatchManager matchManager;

        internal LevelData LevelData { get; }

        internal MatchListingType Type { get; }

        [JsonPropertyName("roomName")]
        internal string Name { get; }

        [JsonPropertyName("levelID")]
        internal uint LevelId => this.LevelData.Id;
        [JsonPropertyName("levelTitle")]
        internal string LevelTitle => this.LevelData.Title;
        [JsonPropertyName("version")]
        internal uint LevelVersion => this.LevelData.Version;

        [JsonPropertyName("creatorID")]
        internal uint CreatorId => this.LevelData.AuthorUserId;
        [JsonPropertyName("creatorName")]
        internal string CreatorName => this.LevelData.AuthorUsername;
        [JsonPropertyName("creatorNameColor")]
        internal Color CreatorNameColor => this.LevelData.AuthorNameColor;

        [JsonPropertyName("levelType")]
        internal LevelMode LevelMod => this.LevelData.Mode;

        [JsonPropertyName("likes")]
        public uint Likes => this.LevelData.Likes;
        [JsonPropertyName("dislikes")]
        public uint Dislikes => this.LevelData.Dislikes;

        [JsonPropertyName("minRank")]
        internal uint MinRank { get; }
        [JsonPropertyName("maxRank")]
        internal uint MaxRank { get; }

        [JsonPropertyName("maxMembers")]
        internal uint MaxMembers { get; }
        internal bool OnlyFriends { get; }

        private ClientSessionCollectionLimited _Clients;
        private ClientSessionCollection LobbyClients;
        private ConcurrentBag<IUserIdentifier> BannedClients;

        private uint _HostSocketId;
        public uint HostSocketId => this._HostSocketId;

        private volatile int UsersReady;

        internal MatchListing(MatchListingManager matchListingManager, MatchManager matchManager, MatchListingType type, ClientSession creator, LevelData levelData, string name, uint minRank, uint maxRank, uint maxMembers, bool onlyFriends)
        {
            this.matchListingManager = matchListingManager;
            this.matchManager = matchManager;

            this._Clients = new ClientSessionCollectionLimited((int)maxMembers, this.Leave0);
            this.LobbyClients = new ClientSessionCollection();
            this.BannedClients = new ConcurrentBag<IUserIdentifier>();

            this.LevelData = levelData;

            this.Name = name;

            this.MinRank = minRank;
            this.MaxRank = maxRank;

            this.MaxMembers = maxMembers;
            this.OnlyFriends = onlyFriends;

            this.Type = type;

            if (creator != null)
            {
                this._HostSocketId = creator.SocketId;

                creator.Connection.OnDisconnected += this.OnCreatorDisconnectEarly;
            }
        }

        internal ICollection<ClientSession> Clients => this._Clients.Sessions;
        internal int ClientsCount => this._Clients.Count;

        private void OnCreatorDisconnectEarly(ISocket connection)
        {
            this.matchListingManager.Die(this); //We can pull the plug no biggie
        }

        internal MatchListingJoinStatus CanJoin(ClientSession session)
        {
            if (this._Clients.IsFull)
            {
                return MatchListingJoinStatus.Full;
            }
            else if (this.HostSocketId == session.SocketId)
            {
                return MatchListingJoinStatus.Success;
            }
            else if (this.Type == MatchListingType.Normal && this._Clients.Count == 0)
            {
                return MatchListingJoinStatus.WaitingForHost;
            }
            else if (this.Type != MatchListingType.LevelOfTheDay && (this.MinRank > session.UserData.Rank || this.MaxRank < session.UserData.Rank) && !session.HasPermissions(Permissions.ACCESS_BYPASS_MATCH_LISTING_RANK_REQUIREMENT))
            {
                return MatchListingJoinStatus.NoRankRequirement;
            }
            else if (this.Type != MatchListingType.LevelOfTheDay && this.OnlyFriends && !session.HasPermissions(Permissions.ACCESS_BYPASS_MATCH_LISTING_ONLY_FRIENDS))
            {
                if (session.IsGuest)
                {
                    return MatchListingJoinStatus.FriendsOnly;
                }
                else if (this._Clients.TryGetValue(this.HostSocketId, out ClientSession host) && !host.IsGuest)
                {
                    return host.UserData.Friends.Contains(session.UserData.Id) ? MatchListingJoinStatus.Success : MatchListingJoinStatus.FriendsOnly;
                }
            }
            else if (this.Type != MatchListingType.LevelOfTheDay && this.BannedClients.Any((i) => i.Matches(session.UserData.Id, session.SocketId, session.IPAddres)))
            {
                return MatchListingJoinStatus.Banned;
            }

            return MatchListingJoinStatus.Success;
        }

        internal bool JoinLobby(ClientSession session)
        {
            if (this.LobbyClients.TryAdd(session))
            {
                //Send the other clients
                foreach (ClientSession other in this._Clients.Sessions)
                {
                    session.TrackUserInRoom(this.Name, other.SocketId, other.UserData.Id, other.UserData.Username, other.GetVars("userName", "rank", "hat", "head", "body", "feet", "hatColor", "headColor", "bodyColor", "feetColor", "socketID", "ping"));
                }

                return true;
            }

            return false;
        }

        internal void LeaveLobby(ClientSession session)
        {
            this.LobbyClients.TryRemove(session);

            session.UntrackUsersInRoom(this.Name);
        }

        internal bool Join(ClientSession session)
        {
            session.Connection.OnDisconnected -= this.OnCreatorDisconnectEarly;

            MatchListingJoinStatus canJoinStatus = this.CanJoin(session);
            if (canJoinStatus != MatchListingJoinStatus.Success)
            {
                return false;
            }

            if (!this._Clients.TryAdd(session))
            {
                return false;
            }

            session.LobbySession.MatchListing = this;
            session.LobbySession.RemoveMatch(this);

            //Send the other clients
            foreach (ClientSession other in this._Clients.Sessions.OrderBy((c) => c == session))
            {
                session.TrackUserInRoom(this.Name, other.SocketId, other.UserData.Id, other.UserData.Username, other.GetVars("userName", "rank", "hat", "head", "body", "feet", "hatColor", "headColor", "bodyColor", "feetColor", "socketID", "ping"));
                other.TrackUserInRoom(this.Name, session.SocketId, session.UserData.Id, session.UserData.Username, session.GetVars("userName", "rank", "hat", "head", "body", "feet", "hatColor", "headColor", "bodyColor", "feetColor", "socketID", "ping"));
            }

            foreach (ClientSession other in this.LobbyClients.Sessions)
            {
                other.TrackUserInRoom(this.Name, session.SocketId, session.UserData.Id, session.UserData.Username, session.GetVars("userName", "rank", "hat", "head", "body", "feet", "hatColor", "headColor", "bodyColor", "feetColor", "socketID", "ping"));
            }

            uint currentHost = this._HostSocketId;
            if (this.Type == MatchListingType.Normal && (currentHost == session.SocketId || (currentHost == 0 && Interlocked.CompareExchange(ref this._HostSocketId, session.SocketId, currentHost) == currentHost)))
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

            this.TryStartFull();

            return true;
        }

        private void TryStartFull()
        {
            //Makes sure everyone has received all the packets before starting
            if (Interlocked.Increment(ref this.UsersReady) == this.MaxMembers)
            {
                //Ensure we can mark this client as full successfully before starting
                if (this._Clients.MarkFullIfFull())
                {
                    this.Start();
                }
            }
        }

        internal void Leave(ClientSession session) => this._Clients.TryRemove(session);

        internal void Kick(ClientSession session)
        {
            this.Leave(session);

            session.SendPacket(new UserLeaveRoomOutgoingMessage(this.Name, session.SocketId));
        }

        private void Leave0(ClientSession session)
        {
            session.LobbySession.MatchListing = null;

            //Concurrency complexity
            Interlocked.Decrement(ref this.UsersReady); //We can freely decrement this here without having issues

            foreach (ClientSession other in this._Clients.Sessions)
            {
                other.UntrackUserInRoom(this.Name, session.SocketId);
            }

            foreach (ClientSession other in this.LobbyClients.Sessions)
            {
                other.UntrackUserInRoom(this.Name, session.SocketId);
            }

            session.UntrackUsersInRoom(this.Name);

            if (this.Type != MatchListingType.LevelOfTheDay)
            {
                if (!this._Clients.MarkFullIf((int)this.MaxMembers))
                {
                    uint currentHost = this._HostSocketId;
                    if (this.Type == MatchListingType.Normal && currentHost == session.SocketId) //Time to pick new host
                    {
                        //TODO: How should we handle friends only?

                        while (this._Clients.Count > 0)
                        {
                            ClientSession other = this._Clients.Sessions.FirstOrDefault();
                            if (other != null)
                            {
                                if (Interlocked.CompareExchange(ref this._HostSocketId, other.SocketId, currentHost) == currentHost)
                                {
                                    other.SendPacket(new MatchOwnerOutgoingMessage(this.Name, true, true, true));
                                    break;
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
                    foreach (ClientSession other in this.LobbyClients.Sessions)
                    {
                        other.LobbySession.RemoveMatch(this);
                    }

                    this.matchListingManager.Die(this);
                }
            }
        }

        internal void ForceStart(ClientSession session)
        {
            if (this.HostSocketId == session.SocketId || session.HasPermissions(Permissions.ACCESS_FORCE_START_ANY_MATCH_LISTING))
            {
                //Mark this full and get the amount of left spots
                int left = this._Clients.MarkFull();
                if (this.MaxMembers - this.UsersReady == left) //If its the same as users ready we can start, otherwise the Join method does this for us when the user is "ready"
                {
                    this.Start();
                }
            }
        }

        internal void Start()
        {
            this.matchListingManager.Die(this); //Kill us :(

            MultiplayerMatch match = this.matchManager.CreateMultiplayerMatch(this);

            Random random = new();

            StartGameOutgoingMessage startGame = new(this.Name, match.Name);
            foreach (ClientSession session in this._Clients.Sessions.OrderBy((s) => random.Next(int.MinValue, int.MaxValue)))
            {
                session.LobbySession.MatchListing = null;

                if (match.Reserve(session))
                {
                    session.SendPacket(startGame);
                }
            }

            match.Lock();

            foreach(ClientSession session in this.LobbyClients.Sessions)
            {
                session.SendPacket(startGame);
                session.UntrackUsersInRoom(this.Name);
            }
        }

        internal IReadOnlyDictionary<string, object> GetVars(params string[] vars) => JsonUtils.GetVars(this, vars);
        internal IReadOnlyDictionary<string, object> GetVars(HashSet<string> vars) => JsonUtils.GetVars(this, vars);

        internal void Kick(ClientSession session, uint socketId)
        {
            if (session.SocketId == this.HostSocketId || session.HasPermissions(Permissions.ACCESS_KICK_ANY_MATCH_LISTING))
            {
                if (this._Clients.TryGetValue(socketId, out ClientSession target))
                {
                    if (target.HasPermission(Permissions.ACCESS_KICK_IMMUNITY_MATCH_LISTING))
                    {
                        if (!(session.UserData is PlayerUserData sessionPlayerUserData) || (target.UserData is PlayerUserData targetPlayerUserData && targetPlayerUserData.PermissionRank > sessionPlayerUserData.PermissionRank))
                        {
                            return;
                        }
                    }

                    this.Kick(target);
                }
            }
        }

        internal void Ban(ClientSession session, uint socketId)
        {
            if (session.SocketId == this.HostSocketId || session.HasPermissions(Permissions.ACCESS_BAN_ANY_MATCH_LISTING))
            {
                if (this._Clients.TryGetValue(socketId, out ClientSession target))
                {
                    if (target.HasPermission(Permissions.ACCESS_KICK_IMMUNITY_MATCH_LISTING))
                    {
                        if (!(session.UserData is PlayerUserData sessionPlayerUserData) || (target.UserData is PlayerUserData targetPlayerUserData && targetPlayerUserData.PermissionRank > sessionPlayerUserData.PermissionRank))
                        {
                            return;
                        }
                    }

                    if (target.IsGuest)
                    {
                        this.BannedClients.Add(new GuestIdentifier(target.SocketId, target.IPAddres));
                    }
                    else
                    {
                        this.BannedClients.Add(new PlayerIdentifier(target.UserData.Id));
                    }

                    this.Kick(target);
                }
            }
        }
    }
}
