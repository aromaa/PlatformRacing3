using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages;
using Platform_Racing_3_Server_API.Net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platform_Racing_3_Server.Collections
{
    internal class ClientSessionCollection
    {
        private ConcurrentDictionary<uint, ClientSession> Clients;

        private Action<ClientSession> OnDisconnectCallback;
        private Action OnDisconnectAction;

        internal ClientSessionCollection()
        {
            this.Clients = new ConcurrentDictionary<uint, ClientSession>();
        }

        internal ClientSessionCollection(Action<ClientSession> callback)
        {
            this.Clients = new ConcurrentDictionary<uint, ClientSession>();

            this.OnDisconnectCallback = callback;
        }

        public ClientSessionCollection(Action onDisconnect)
        {
            this.OnDisconnectAction = onDisconnect;
        }

        internal uint Count => (uint)this.Clients.Count;
        internal ICollection<ClientSession> Values => this.Clients.Values;

        public bool Add(ClientSession session)
        {
            if (this.Clients.TryAdd(session.SocketId, session))
            {
                session.OnDisconnect += this.OnDisconnect;
                if (session.Disconnected)
                {
                    this.Remove(session);
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        private void OnDisconnect(INetworkConnection networkConnection)
        {
            if (this.Remove(networkConnection.SocketId, out ClientSession session))
            {
                this.OnDisconnectCallback?.Invoke(session);
            }
        }

        public bool Remove(ClientSession session)
        {
            session.OnDisconnect -= this.OnDisconnect;

            return this.Clients.TryRemove(session.SocketId, out _);
        }

        public bool Remove(uint socketId, out ClientSession session)
        {
            if (this.Clients.TryRemove(socketId, out session))
            {
                session.OnDisconnect -= this.OnDisconnect;

                return true;
            }

            return false;
        }

        public bool Contains(uint socketId) => this.Clients.ContainsKey(socketId);
        public bool Contains(ClientSession session) => this.Clients.ContainsKey(session.SocketId);
        public bool TryGetClientSession(uint socketId, out ClientSession session) => this.Clients.TryGetValue(socketId, out session);

        public void SendPacket(IMessageOutgoing packet)
        {
            foreach(ClientSession session in this.Clients.Values)
            {
                session.SendPacket(packet);
            }
        }

        public void SendPacket(IMessageOutgoing packet, params ClientSession[] ignore)
        {
            foreach (ClientSession session in this.Clients.Values.Except(ignore))
            {
                session.SendPacket(packet);
            }
        }

        public void SendPackets(params IMessageOutgoing[] packets)
        {
            foreach (ClientSession session in this.Clients.Values)
            {
                session.SendPackets(packets);
            }
        }
    }
}
