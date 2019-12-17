using Net.Communication.Incoming.Helpers;
using Net.Communication.Incoming.Packet;
using Net.Communication.Incoming.Packet.Consumer;
using Net.Communication.Incoming.Packet.Handler;
using Net.Communication.Incoming.Packet.Parser;
using Net.Communication.Managers;
using Net.Communication.Pipeline;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Managers.Consumer;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Managers
{
    internal class BytePacketManager : PacketManager<uint>
    {
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

        public bool HandleIncomingData(uint packetId, ClientSession session, ref SocketPipelineContext context, ref PacketReader reader)
        {
            if (this.TryGetConsumer(packetId, out IIncomingPacketConsumer consumer))
            {
                if (consumer is IClientSessionIncomingPacketConsumer sessionConsumer)
                {
                    sessionConsumer.Read(session, ref reader);
                }
                else
                {
                    consumer.Read(ref context, ref reader);
                }

                return true;
            }

            return false;
        }
    }
}
