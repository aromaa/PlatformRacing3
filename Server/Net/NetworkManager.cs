using log4net;
using Platform_Racing_3_Server.Game.Communication.Messages;
using Platform_Racing_3_Server.Net.TCP;
using Platform_Racing_3_Server_API.Net;
using Platform_Racing_3_UnsafeHelpers.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Platform_Racing_3_Server.Net
{
    internal class NetworkManager : INetworkManager
    {
        private const uint TimeoutTime = 30;

        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private volatile bool Disposed;
        private uint NextSessionId;

        private BlockingCollection<INetworkListener> NetworkListeners;
        private ConcurrentDictionary<uint, INetworkConnection> NetworkConnections;

        internal PacketManager PacketManager { get; }

        private Timer TimeoutConnectionTimer;

        internal NetworkManager()
        {
            this.NextSessionId = 0;

            this.NetworkListeners = new BlockingCollection<INetworkListener>();
            this.NetworkConnections = new ConcurrentDictionary<uint, INetworkConnection>();

            this.PacketManager = new PacketManager();

            this.TimeoutConnectionTimer = new Timer(this.CheckForTimedoutConnections, null, 20000, 20000);
        }

        private void CheckForTimedoutConnections(object state)
        {
            foreach(INetworkConnection connection in this.NetworkConnections.Values)
            {
                if (connection.LastRead.TotalSeconds >= NetworkManager.TimeoutTime)
                {
                    connection.Disconnect("Timeout");
                }
            }
        }

        private uint GetNextSessionId() => InterlockedExtansions.Increment(ref this.NextSessionId);
        
        public void AddListener(INetworkListener listener, bool start)
        {
            if (this.Disposed)
            {
                throw new ObjectDisposedException(nameof(NetworkManager));
            }

            this.NetworkListeners.Add(listener);

            NetworkManager.Logger.Info($"Listener binded on {listener.Bind} was added");

            if (start)
            {
                listener.StartListening();
            }
        }

        internal bool HandleIncomingConnectionTCP(Socket socket, out NetworkConnectionTCP networkConnection)
        {
            if (this.Disposed)
            {
                throw new ObjectDisposedException(nameof(NetworkManager));
            }

            networkConnection = new NetworkConnectionTCP(this.GetNextSessionId(), socket);
            if (this.NetworkConnections.TryAdd(networkConnection.SocketId, networkConnection))
            {
                return !this.Disposed;
            }

            return false;
        }

        internal void HandleDisconnection(uint socketId)
        {
            this.NetworkConnections.TryRemove(socketId, out _);
        }

        public void Shutdown()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            if (!this.Disposed)
            {
                this.Disposed = true;

                this.TimeoutConnectionTimer.Dispose();

                this.NetworkListeners.CompleteAdding();
                foreach(INetworkListener listener in this.NetworkListeners)
                {
                    listener.StopListening();
                }

                this.NetworkListeners.Dispose();
                
                while (this.NetworkConnections.Count > 0) //Make sure EVERYONE is gone
                {
                    foreach (uint sessionId in this.NetworkConnections.Keys)
                    {
                        if (this.NetworkConnections.TryRemove(sessionId, out INetworkConnection connection))
                        {
                            connection.Disconnect("Network manager shutdown");
                        }
                    }
                }
            }
        }

        public ICollection<INetworkConnection> GetActiveConnections() => this.NetworkConnections.Values;
    }
}
