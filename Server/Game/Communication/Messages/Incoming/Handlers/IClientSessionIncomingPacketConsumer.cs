using Net.Communication.Incoming.Helpers;
using Net.Communication.Incoming.Packet.Consumer;
using Net.Communication.Pipeline;
using Platform_Racing_3_Server.Game.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Handlers
{
    internal interface IClientSessionIncomingPacketConsumer : IIncomingPacketConsumer
    {
        void IIncomingPacketConsumer.Read(ref SocketPipelineContext context, ref PacketReader reader) => throw new NotSupportedException();

        void Read(ClientSession session, ref PacketReader reader);
    }
}
