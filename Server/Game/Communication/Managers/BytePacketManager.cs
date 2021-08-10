using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Managers.Consumer;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Handlers;
using System;
using System.Collections.Generic;
using System.Text;
using Net.Buffers;
using Net.Communication.Incoming.Consumer;
using Net.Communication.Incoming.Handler;
using Net.Communication.Incoming.Parser;
using Net.Communication.Manager;
using Net.Sockets.Pipeline.Handler;

namespace Platform_Racing_3_Server.Game.Communication.Managers
{
    internal sealed class BytePacketManager : PacketManager<uint>
    {
        public BytePacketManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        protected override IIncomingPacketConsumer BuildConsumer(Type type, IIncomingPacketParser parser, IIncomingPacketHandler handler)
        {
            if (handler == null || !typeof(IClientSessionPacketHandler).IsAssignableFrom(handler.GetType()))
            {
                return base.BuildConsumer(type, parser, handler);
            }

            Type consumerType = typeof(IncomingClientSessionPacketConsumer<>);
            consumerType = consumerType.MakeGenericType(type);

            return (IIncomingPacketConsumer)Activator.CreateInstance(consumerType, new object[]
            {
                parser,
                handler
            })!;
        }

        public bool HandleIncomingData(uint packetId, ClientSession session, IPipelineHandlerContext context, ref PacketReader reader)
        {
            if (this.TryGetConsumer(packetId, out IIncomingPacketConsumer consumer))
            {
                if (consumer is IClientSessionIncomingPacketConsumer sessionConsumer)
                {
                    sessionConsumer.Read(session, ref reader);
                }
                else
                {
                    context.ProgressReadHandler(ref reader);
                }

                return true;
            }

            return false;
        }
    }
}
