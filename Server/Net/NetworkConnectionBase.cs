using log4net;
using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Extensions;
using Platform_Racing_3_Server.Game.Communication.Handlers;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing;
using Platform_Racing_3_Server_API.Net;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace Platform_Racing_3_Server.Net
{
    public abstract class NetworkConnectionBase : INetworkConnection
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private protected abstract int BufferSize { get; }

        private volatile bool SentDisconnectReason;
        private volatile bool Disposed;
        private volatile bool Listening;
        public bool Disconnected => this.Disposed;

        public uint SocketId { get; }
        public IPAddress RemoteAddress { get; }

        private Socket Socket { get; }
        private byte[] Buffer;

        private AsyncCallback ReceiveCallback;
        private AsyncCallback SendCallback;

        private Stopwatch _LastRead;
        
        public event NetworkEvents.OnDisconnect OnDisconnect;

        public NetworkConnectionBase(uint socketId, Socket socket)
        {
            this.SocketId = socketId;
            this.RemoteAddress = IPAddress.Parse(socket.RemoteEndPoint.ToString().Split(':')[0]);

            this.Socket = socket;

            this._LastRead = Stopwatch.StartNew();
        }

        public TimeSpan LastRead => this._LastRead.Elapsed;

        public void StartListening()
        {
            if (!this.Disposed && !this.Listening)
            {
                this.Listening = true;

                this.Buffer = ArrayPool<byte>.Shared.Rent(this.BufferSize);
                this.ReceiveCallback = new AsyncCallback(this.Received);
                this.SendCallback = new AsyncCallback(this.Sent);

                this.PostStartListening();

                this.BeginReceive();
            }
        }

        private protected abstract void PostStartListening();

        private void BeginReceive()
        {
            if (!this.Disposed && this.Listening)
            {
                try
                {
                    this.Socket.BeginReceive(this.Buffer, 0, this.Buffer.Length, SocketFlags.None, out SocketError error, this.ReceiveCallback, this.Socket);
                    if (error.DisconnectFor())
                    {
                        this.Disconnect("Failed to receive: " + error);
                    }
                }
                catch
                {
                    this.Disconnect("Failed to start receive");
                }
            }
        }

        private void Received(IAsyncResult asyncResult)
        {
            if (!this.Disposed && this.Listening)
            {
                try
                {
                    this._LastRead.Restart();

                    int numReceived = this.Socket.EndReceive(asyncResult, out SocketError error);
                    if (!error.DisconnectFor())
                    {
                        if (numReceived > 0)
                        {
                            try
                            {
                                this.HandleData(this.Buffer.AsSpan().Slice(0, numReceived));
                            }
                            catch(Exception ex)
                            {
                                this.Disconnect("Failed to handle data");

                                NetworkConnectionBase.Logger.Error("Failed to handle data", ex);
                            }
                        }
                        else
                        {
                            this.Disconnect("Received zero bytes (Disconnect)");
                        }
                    }
                    else
                    {
                        this.Disconnect("Failed to receive: " + error);
                    }
                }
                catch
                {
                    this.Disconnect("Failed to receive data");
                }
                finally
                {
                    this.BeginReceive();
                }
            }
        }

        private protected abstract void HandleData(Span<byte> data);

        public void Send(byte[] data, int offset = 0, int? length = default)
        {
            if (!this.Disposed && this.Listening)
            {
                try
                {
                    if (length == default)
                    {
                        length = data.Length;
                    }

                    this.Socket.BeginSend(data, 0, (int)length, SocketFlags.None, out SocketError error, this.SendCallback, this.Socket);
                    if (error.DisconnectFor())
                    {
                        this.Disconnect("Failed to send: " + error);
                    }
                }
                catch
                {
                    this.Disconnect("Failed to send data");
                }
            }
        }

        private void Sent(IAsyncResult asyncResult)
        {
            if (!this.Disposed && this.Listening)
            {
                try
                {
                    this.Socket.EndSend(asyncResult, out SocketError error);
                    if (error.DisconnectFor())
                    {
                        this.Disconnect("Failed to sent: " + error);
                    }
                }
                catch
                {
                    this.Disconnect("Failed to sent data");
                }
            }
        }

        public void Disconnect(string reason = default)
        {
            if (!this.SentDisconnectReason)
            {
                this.SentDisconnectReason = true;

                NetworkConnectionBase.Logger.Info($"[{this.SocketId}] was disconnected for reason: {reason}");

                this.Send(new LogoutTriggerOutgoingMessage(reason).GetBytes());
            }
            else
            {
                NetworkConnectionBase.Logger.Debug($"[{this.SocketId}] got disconencted for second time for reason: {reason}");
            }

            this.Dispose();
        }

        public void Dispose()
        {
            if (!this.Disposed)
            {
                this.Disposed = true;
                this.Listening = false;

                try
                {
                    this.OnDisconnect?.Invoke(this);
                }
                catch
                {
                }

                PlatformRacing3Server.NetworkManager.HandleDisconnection(this.SocketId);

                try
                {
                    this.Socket.Shutdown(SocketShutdown.Both);
                }
                catch
                {
                }

                this.Socket.Close();

                if (this.Buffer != null)
                {
                    ArrayPool<byte>.Shared.Return(this.Buffer);
                }
            }
        }
    }
}
