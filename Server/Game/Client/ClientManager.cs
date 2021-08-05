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

        private readonly ClientSessionCollection ClientsBySocketId;
        private readonly ConcurrentDictionary<uint, ClientSession> ClientsByUserId;

        private Timer LastPingCheckTimer;

        internal ClientManager()
        {
            this.ClientsBySocketId = new ClientSessionCollection(this.OnAdded, this.OnRemoved);
            this.ClientsByUserId = new ConcurrentDictionary<uint, ClientSession>();

            this.LastPingCheckTimer = new Timer(this.CheckForTimedoutConnections, null, 2500, 2500);
        }

        internal int Count => this.ClientsBySocketId.Count;
        internal ICollection<ClientSession> LoggedInUsers => this.ClientsBySocketId.Sessions;

        internal void Add(ClientSession session)
        {
            this.ClientsBySocketId.TryAdd(session);
        }

        internal bool TryGetClientSessionBySocketId(uint socketId, out ClientSession session) => this.ClientsBySocketId.TryGetValue(socketId, out session);
        internal bool TryGetClientSessionByUserId(uint userId, out ClientSession session) => this.ClientsByUserId.TryGetValue(userId, out session);

        internal ClientSession GetClientSessionByUsername(string username) => this.ClientsByUserId.Values.FirstOrDefault((c) => c.UserData?.Username.ToUpperInvariant() == username.ToUpperInvariant());

        private void OnAdded(ClientSession session)
        {
            if (!session.IsGuest)
            {
                this.ClientsByUserId.AddOrUpdate(session.UserData.Id, session, (oldKey, oldValue) =>
                {
                    oldValue.Disconnect("Logged in from another location");

                    return session;
                });
            }
        }

        private void OnRemoved(ClientSession session)
        {
            if (!session.IsGuest)
            {
                this.ClientsByUserId.TryRemove(new KeyValuePair<uint, ClientSession>(session.UserData.Id, session));
            }
        }

        private void CheckForTimedoutConnections(object state)
        {
            foreach (ClientSession session in this.ClientsBySocketId.Sessions)
            {
                if (session.LastPing.Elapsed.TotalSeconds >= ClientManager.TimeoutTime)
                {
                    session.Disconnect("Timeout (No ping)");
                }
            }
        }
    }
}
