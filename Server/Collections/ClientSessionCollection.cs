using Net.Collections;
using Net.Connections;
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
    internal class ClientSessionCollection : ClientCollectionMetadatable<ClientSession>
    {
        private ConcurrentDictionary<uint, ClientSession> _Sessions;

        private Action<ClientSession, CilentCollectionRemoveReason> RemoveCallback;

        internal ClientSessionCollection() : this(null)
        {
        }

        internal ClientSessionCollection(Action<ClientSession, CilentCollectionRemoveReason> callback)
        {
            this._Sessions = new ConcurrentDictionary<uint, ClientSession>();

            this.RemoveCallback = callback;
        }

        internal bool TryAdd(ClientSession session) => this.TryAdd(session.Connection, session);

        internal bool TryRemove(ClientSession session) => this.TryRemove(session.Connection);

        protected override void OnAdded(SocketConnection connection, ClientSession metadata)
        {
            if (!this._Sessions.TryAdd(connection.Id, metadata))
            {
                throw new InvalidOperationException();
            }
        }

        protected override void OnRemoved(SocketConnection connection, CilentCollectionRemoveReason reason)
        {
            if (this._Sessions.TryRemove(connection.Id, out ClientSession session))
            {
                this.RemoveCallback?.Invoke(session, reason);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public bool Contains(ClientSession session) => this.Contains(session.Connection);

        public bool TryGetValue(uint id, out ClientSession session) => this._Sessions.TryGetValue(id, out session);

        public ICollection<ClientSession> Sessions => this._Sessions.Values;
    }
}
