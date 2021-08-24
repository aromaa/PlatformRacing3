using Platform_Racing_3_Server.Game.Client;
using System;
using System.Collections.Generic;
using System.Text;
using Net.Communication.Incoming.Handler;
using Net.Sockets.Pipeline.Handler;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Handlers
{
    internal abstract class AbstractIncomingClientSessionPacketHandler<T> : IClientSessionPacketHandler, IIncomingPacketHandler<T>
    {
        public void Handle(IPipelineHandlerContext context, in T packet) => throw new NotSupportedException();

        internal abstract void Handle(ClientSession session, in T packet);
    }
}
