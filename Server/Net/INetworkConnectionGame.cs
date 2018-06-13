using Platform_Racing_3_Server.Game.Communication.Handlers;
using Platform_Racing_3_Server.Game.Communication.Messages;
using Platform_Racing_3_Server_API.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Net
{
    internal interface INetworkConnectionGame : INetworkConnection
    {
        IDataHandler<INetworkConnectionGame> DataHandler { get; set; }

        void SendPacket(IMessageOutgoing messageOutgoing);
        void SendPackets(IEnumerable<IMessageOutgoing> messageOutgoing);
        void SendPackets(params IMessageOutgoing[] messageOutgoing);
    }
}
