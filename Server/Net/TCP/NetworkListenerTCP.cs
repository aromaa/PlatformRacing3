using log4net;
using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server_API.Net;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace Platform_Racing_3_Server.Net.TCP
{
    internal class NetworkListenerTCP : INetworkListener
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private volatile bool Disposed;
        private volatile bool Listening;

        public IPEndPoint Bind { get; }
        public int Backlog { get; }

        private Socket Socket;
        private AsyncCallback AcceptCallback;

        public NetworkListenerTCP(IPEndPoint bind, int backlog)
        {
            this.Bind = bind;
            this.Backlog = backlog;

            this.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                NoDelay = true
            };

            this.Socket.Bind(bind);
            this.Socket.Listen(backlog);

            this.AcceptCallback = new AsyncCallback(this.Accept);
        }

        public void StartListening()
        {
            if (this.Disposed)
            {
                throw new ObjectDisposedException(nameof(NetworkListenerTCP));
            }

            if (!this.Listening)
            {
                this.Listening = true;

                NetworkListenerTCP.Logger.Info($"Starting listening on {this.Bind}");

                this.BeginAccept();
            }
        }

        private bool BeginAccept()
        {
            if (!this.Disposed && this.Listening)
            {
                try
                {
                    this.Socket.BeginAccept(this.Accept, this.Socket);

                    return true;
                }
                catch (Exception ex)
                {
                    NetworkListenerTCP.Logger.Error("Failed to being accept", ex);
                }
            }

            return false;
        }

        public void StopListening()
        {
            if (this.Disposed)
            {
                throw new ObjectDisposedException(nameof(NetworkListenerTCP));
            }

            if (this.Listening)
            {
                this.Listening = false;

                NetworkListenerTCP.Logger.Info($"No longer listening on {this.Bind}");
            }
        }

        public void Accept(IAsyncResult asyncResult)
        {
            if (!this.Disposed && this.Listening)
            {
                try
                {
                    Socket socket = (asyncResult.AsyncState as Socket)?.EndAccept(asyncResult);
                    if (socket != null)
                    {
                        bool result = PlatformRacing3Server.NetworkManager.HandleIncomingConnectionTCP(socket, out NetworkConnectionTCP networkConnectionTCP);
                        
                        try
                        {
                            if (result)
                            {
                                NetworkListenerTCP.Logger.Info($"Connection [{networkConnectionTCP.SocketId}] from {networkConnectionTCP.RemoteAddress}");

                                networkConnectionTCP.StartListening();
                            }
                            else
                            {
                                networkConnectionTCP.Disconnect("Connection was refused");
                            }
                        }
                        catch (Exception ex)
                        {
                            networkConnectionTCP.Disconnect("Failed to handle incoming connection");

                            NetworkListenerTCP.Logger.Error("Failed to handle incoming connection", ex);
                        }
                    }
                    else
                    {
                        NetworkListenerTCP.Logger.Warn("Accepted connection socket was null?");
                    }
                }
                catch(Exception ex)
                {
                    NetworkListenerTCP.Logger.Error("Failed to accept incoming connection", ex);
                }
                finally
                {
                    this.BeginAccept();
                }
            }
        }

        public void Dispose()
        {
            if (!this.Disposed)
            {
                this.Disposed = true;
                this.Listening = false;
            }
        }
    }
}
