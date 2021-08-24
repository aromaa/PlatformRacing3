using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Net.Sockets;
using PlatformRacing3.Common.Database;
using PlatformRacing3.Common.User;
using PlatformRacing3.Common.Utils;
using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Communication.Messages;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing;
using PlatformRacing3.Server.Game.Lobby;
using PlatformRacing3.Server.Game.Match;

namespace PlatformRacing3.Server.Game.Client
{
    internal class ClientSession : ICommandExecutor
    {
        internal ISocket Connection { get; }

        private ClientStatus ClientStatus { get; set; }

        internal UserData UserData { get; set; }

        internal Stopwatch LastPing { get; }
        [JsonPropertyName("ping")]
        internal uint LastRoundtripTime { get; set; }

        internal bool HostTournament { get; set; }

        private Lazy<LobbySession> _LobbySession;

        internal MultiplayerMatchSession MultiplayerMatchSession { get; set; }

        private Dictionary<string, HashSet<uint>> TrackingUsersInRoom;
        private Dictionary<string, Dictionary<uint, Queue<IMessageOutgoing>>> TrackingUserData;

        internal ClientSession(ISocket connection)
        {
            this.Connection = connection;
            this.ClientStatus = ClientStatus.None;

            this.LastPing = Stopwatch.StartNew();

            this._LobbySession = new Lazy<LobbySession>(this.SetupLobbySession);

            this.TrackingUsersInRoom = new Dictionary<string, HashSet<uint>>();
            this.TrackingUserData = new Dictionary<string, Dictionary<uint, Queue<IMessageOutgoing>>>();

            this.Connection.OnDisconnected += this.OnDisconnect0;
        }

        private void OnDisconnect0(ISocket connection)
        {
            if (this.UserData != null)
            {
                this.UserData.SetServer(null); //Race condition

                if (!this.UserData.IsGuest)
                {
                    //More of temp solution
                    DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ExecuteNonQueryAsync($"UPDATE base.users SET current_hat = {(uint)this.UserData.CurrentHat}, current_hat_color = {this.UserData.CurrentHatColor.ToArgb()}, current_head = {(uint)this.UserData.CurrentHead}, current_head_color = {this.UserData.CurrentHeadColor.ToArgb()}, current_body = {(uint)this.UserData.CurrentBody}, current_body_color = {this.UserData.CurrentBodyColor.ToArgb()}, current_feet = {(uint)this.UserData.CurrentFeet}, current_feet_color = {this.UserData.CurrentFeetColor.ToArgb()}, speed = {this.UserData.Speed}, accel = {this.UserData.Accel}, jump = {this.UserData.Jump}, last_online = NOW() WHERE id = {this.UserData.Id}"));
                }
            }
        }

        internal bool Disconnected => this.Connection.Closed;
        [JsonPropertyName("socketID")]
        internal uint SocketId => (uint)this.Connection.Id.GetHashCode(); //Relying on internal details, lmao, how bad
        internal IPAddress IPAddres => (this.Connection.RemoteEndPoint as IPEndPoint).Address;

        internal bool IsLoggedIn => this.ClientStatus == ClientStatus.LoggedIn;
        internal bool IsGuest => this.UserData?.IsGuest ?? true;

        internal bool HasPermissions(string permission) => this.UserData?.HasPermissions(permission) ?? false;

        public uint PermissionRank => (this.UserData as PlayerUserData)?.PermissionRank ?? 0;

        private LobbySession SetupLobbySession => new(this);
        private object[] SetupVarsObject() => new object[] { this, this.UserData };

        public LobbySession LobbySession => this._LobbySession.Value;

        internal void SendPacket(IMessageOutgoing messageOutgoing) => this.Connection.SendAsync(messageOutgoing);

        internal void Disconnect(string reason = null)
        {
            _ = TriggerDisconnect(reason);

            async Task TriggerDisconnect(string reason)
            {
                await this.Connection.SendAsync(new LogoutTriggerOutgoingMessage(reason));

                this.Connection.Disconnect(reason);
            }
        }

        internal bool UpgradeClientStatus(ClientStatus clientStatus)
        {
            if (clientStatus > this.ClientStatus)
            {
                this.ClientStatus = clientStatus;

                return true;
            }

            return false;
        }

        internal void TrackUserInRoom(string roomName, uint socketId, uint userId, string username, IReadOnlyDictionary<string, object> vars)
        {
            lock (((IDictionary)this.TrackingUsersInRoom).SyncRoot)
            {
                if (!this.TrackingUsersInRoom.TryGetValue(roomName, out HashSet<uint> users))
                {
                    this.TrackingUsersInRoom[roomName] = users = new HashSet<uint>();
                }

                if (users.Add(socketId))
                {
                    this.SendPacket(new UserJoinRoomOutgoingMessage(roomName, socketId, userId, username, vars));
                }

                if (this.TrackingUserData.TryGetValue(roomName, out Dictionary<uint, Queue<IMessageOutgoing>> data))
                {
                    if (data.TryGetValue(socketId, out Queue<IMessageOutgoing> queuedData))
                    {
                        while (queuedData.TryDequeue(out IMessageOutgoing outgoing))
                        {
                            this.SendPacket(outgoing);
                        }
                    }
                }
            }
        }

        internal void UntrackUserInRoom(string roomName, uint socketId)
        {
            lock (((IDictionary)this.TrackingUsersInRoom).SyncRoot)
            {
                if (this.TrackingUsersInRoom.TryGetValue(roomName, out HashSet<uint> users))
                {
                    if (users.Remove(socketId))
                    {
                        this.SendPacket(new UserLeaveRoomOutgoingMessage(roomName, socketId));
                    }
                }

                if (this.TrackingUserData.TryGetValue(roomName, out Dictionary<uint, Queue<IMessageOutgoing>> data))
                {
                    data.Remove(socketId);
                }
            }
        }

        internal void UntrackUsersInRoom(string roomName)
        {
            lock (((IDictionary)this.TrackingUsersInRoom).SyncRoot)
            {
                this.TrackingUsersInRoom.Remove(roomName);
                this.TrackingUserData.Remove(roomName);
            }
        }

        internal void SendUserRoomData(string roomName, uint socketId, IMessageOutgoing outgoing)
        {
            lock (((IDictionary)this.TrackingUsersInRoom).SyncRoot)
            {
                if (this.TrackingUsersInRoom.TryGetValue(roomName, out HashSet<uint> trackingUsers))
                {
                    if (trackingUsers.Contains(socketId))
                    {
                        this.SendPacket(outgoing);
                        return;
                    }
                }

                if (!this.TrackingUserData.TryGetValue(roomName, out Dictionary<uint, Queue<IMessageOutgoing>> data))
                {
                    this.TrackingUserData[roomName] = data = new Dictionary<uint, Queue<IMessageOutgoing>>();
                }

                if (!data.TryGetValue(socketId, out Queue<IMessageOutgoing> queuedData))
                {
                    data[socketId] = queuedData = new Queue<IMessageOutgoing>();
                }

                queuedData.Enqueue(outgoing);
            }
        }

        public void SendMessage(string message)
        {
            this.SendPacket(new AlertOutgoingMessage(message));
        }

        internal IReadOnlyDictionary<string, object> GetVars(params string[] vars) => this.GetVars(vars.ToHashSet());
        internal IReadOnlyDictionary<string, object> GetVars(HashSet<string> vars)
        {
            Dictionary<string, object> userVars = new();

            JsonUtils.GetVars(this.UserData, vars, userVars);
            JsonUtils.GetVars(this, vars, userVars);

            return userVars;
        }

        public bool HasPermission(string permission) => this.UserData?.HasPermissions(permission) ?? false;
    }
}
