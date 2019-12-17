using Net.Communication.Incoming.Packet;
using Net.Communication.Incoming.Packet.Handler;
using Net.Communication.Pipeline;
using Platform_Racing_3_Server.Game.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Handlers
{
    internal abstract class AbstractIncomingClientSessionPacketHandler<T> : IClientSessionPacketHandler, IIncomingPacketHandler<T>
    {
        public void Handle(ref SocketPipelineContext context, in T packet) => throw new NotSupportedException();

        internal abstract void Handle(ClientSession session, in T packet);
    }
}
