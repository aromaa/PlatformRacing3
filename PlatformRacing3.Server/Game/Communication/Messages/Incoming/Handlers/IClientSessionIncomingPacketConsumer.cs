using System;
using Net.Buffers;
using Net.Communication.Incoming.Consumer;
using Net.Sockets.Pipeline.Handler;
using PlatformRacing3.Server.Game.Client;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Handlers
{
    internal interface IClientSessionIncomingPacketConsumer : IIncomingPacketConsumer
    {
        void IIncomingPacketConsumer.Read(IPipelineHandlerContext context, ref PacketReader reader) => throw new NotSupportedException();

        void Read(ClientSession session, ref PacketReader reader);
    }
}
