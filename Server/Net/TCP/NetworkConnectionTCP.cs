using log4net;
using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Extensions;
using Platform_Racing_3_Server.Game.Communication.Handlers;
using Platform_Racing_3_Server.Game.Communication.Messages;
using Platform_Racing_3_Server_API.Net;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Linq;

namespace Platform_Racing_3_Server.Net.TCP
{
    internal class NetworkConnectionTCP : NetworkConnectionBase, INetworkConnectionGame
    {
        private protected override int BufferSize => 1024;

        public NetworkConnectionTCP(uint socketId, Socket socket) : base(socketId, socket)
        {
        }

        public IDataHandler<INetworkConnectionGame> DataHandler { get; set; }

        private protected override void HandleData(Span<byte> data)
        {
            this.DataHandler.HandleData(this, data);
        }

        private protected override void PostStartListening()
        {
            this.DataHandler = SocketAwakeDataHandler.Instance;
        }

        public void SendPacket(IMessageOutgoing messageOutgoing)
        {
            this.Send(messageOutgoing.GetBytes());
        }

        public void SendPackets(params IMessageOutgoing[] messagesOutgoing)
        {
            foreach(IMessageOutgoing outgoing in messagesOutgoing)
            {
                this.SendPacket(outgoing);
            }
        }

        public void SendPackets(IEnumerable<IMessageOutgoing> messagesOutgoing)
        {
            foreach (IMessageOutgoing outgoing in messagesOutgoing)
            {
                this.SendPacket(outgoing);
            }
        }
    }
}
