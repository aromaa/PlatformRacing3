using Platform_Racing_3_Server.Collections;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Platform_Racing_3_Server.Game.Client
{
    internal class ClientManager
    {
        private const uint TimeoutTime = 10;

        private ClientSessionCollection ClientsBySocketId;
        private ConcurrentDictionary<uint, ClientSession> ClientsByUserId;

        private Timer LastPingCheckTimer;

        internal ClientManager()
        {
            this.ClientsBySocketId = new ClientSessionCollection(this.OnDisconnect);
            this.ClientsByUserId = new ConcurrentDictionary<uint, ClientSession>();

            this.LastPingCheckTimer = new Timer(this.CheckForTimedoutConnections, null, 2500, 2500);
        }

        private void CheckForTimedoutConnections(object state)
        {
            foreach(ClientSession session in this.ClientsBySocketId.Values)
            {
                if (session.LastPing.Elapsed.TotalSeconds >= ClientManager.TimeoutTime)
                {
                    session.Disconnect("Timeout (No ping)");
                }
            }
        }

        private void OnDisconnect(ClientSession session)
        {
            if (!session.IsGuest)
            {
                this.ClientsByUserId.TryRemove(session.UserData.Id, out _);
            }
        }

        internal void Add(ClientSession session)
        {
            if (!session.IsGuest)
            {
                this.ClientsByUserId.AddOrUpdate(session.UserData.Id, session, (oldKey, oldValue) =>
                {
                    oldValue.Disconnect("Logged in from another location");

                    return session;
                });
            }

            this.ClientsBySocketId.Add(session);
        }

        internal bool TryGetClientSessionBySocketId(uint socketId, out ClientSession session) => this.ClientsBySocketId.TryGetClientSession(socketId, out session);
        internal bool TryGetClientSessionByUserId(uint userId, out ClientSession session) => this.ClientsByUserId.TryGetValue(userId, out session);

        internal ClientSession GetClientSessionByUsername(string username) => this.ClientsByUserId.Values.FirstOrDefault((c) => c.UserData?.Username.ToUpperInvariant() == username.ToUpperInvariant());

        internal ICollection<ClientSession> GetLoggedInUsers() => this.ClientsBySocketId.Values;
        internal uint Count => this.ClientsBySocketId.Count;
    }
}
