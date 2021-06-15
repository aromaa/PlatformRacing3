using Platform_Racing_3_Server.Game.Client;
using System;
using System.Collections.Generic;
using System.Text;
using Net.Buffers;
using Net.Communication.Incoming.Consumer;
using Net.Sockets.Pipeline.Handler;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Handlers
{
    internal interface IClientSessionIncomingPacketConsumer : IIncomingPacketConsumer
    {
        void IIncomingPacketConsumer.Read(IPipelineHandlerContext context, ref PacketReader reader) => throw new NotSupportedException();

        void Read(ClientSession session, ref PacketReader reader);
    }
}
