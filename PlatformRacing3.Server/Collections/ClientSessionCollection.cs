using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Net.Collections;
using Net.Sockets;
using PlatformRacing3.Server.Collections.Matcher;
using PlatformRacing3.Server.Game.Client;

namespace PlatformRacing3.Server.Collections
{
    internal class ClientSessionCollection
    {
        private readonly CriticalSocketCollection<ClientSession> SessionCollection;
        private readonly ConcurrentDictionary<uint, ClientSession> SessionsBySocketId;

        private readonly Action<ClientSession> AddCallback;
        private readonly Action<ClientSession> RemoveCallback;

        internal ClientSessionCollection(Action<ClientSession> addCallback = null, Action<ClientSession> removeCallback = null)
        {
            this.SessionCollection = new CriticalSocketCollection<ClientSession>(this.OnAdded, this.OnRemoved);
            this.SessionsBySocketId = new ConcurrentDictionary<uint, ClientSession>();

            this.AddCallback = addCallback;
            this.RemoveCallback = removeCallback;
        }

        internal int Count => this.Sessions.Count;

        internal ICollection<ClientSession> Sessions => this.SessionsBySocketId.Values;

        internal virtual bool TryAdd(ClientSession session) => this.SessionCollection.TryAdd(session.Connection, session, callEvent: true);
        internal virtual bool TryRemove(ClientSession session, bool callEvent = true) => this.SessionCollection.TryRemove(session.Connection, out _, callEvent);

        internal bool Contains(ClientSession session) => this.SessionCollection.Contains(session.Connection);

        internal bool TryGetValue(uint id, out ClientSession session) => this.SessionsBySocketId.TryGetValue(id, out session);

        internal Task SendAsync<TPacket>(in TPacket packet) => this.SessionCollection.SendAsync(packet);
        internal Task SendAsync<TPacket>(in TPacket packet, ClientSession session) => this.SessionCollection.SendAsync(packet, new ExcludeSocketMatcher(session.Connection));

        protected virtual void OnAdded(ISocket socket, ref ClientSession session)
        {
            this.SessionsBySocketId.TryAdd(session.SocketId, session);

            this.AddCallback?.Invoke(session);
        }

        protected virtual void OnRemoved(ISocket socket, ref ClientSession session)
        {
            this.SessionsBySocketId.TryRemove(session.SocketId, out _);

            this.RemoveCallback?.Invoke(session);
        }
    }
}
