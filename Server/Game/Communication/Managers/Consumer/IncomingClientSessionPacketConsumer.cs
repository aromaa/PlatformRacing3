using Net.Communication.Incoming.Helpers;
using Net.Communication.Incoming.Packet.Consumer;
using Net.Communication.Incoming.Packet.Handler;
using Net.Communication.Incoming.Packet.Parser;
using Net.Communication.Pipeline;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Managers.Consumer
{
    internal class IncomingClientSessionPacketConsumer<T> : AbstractIncomingClientSessionPacketHandler<T>, IClientSessionIncomingPacketConsumer, IIncomingPacketParser<T>
    {
        public IIncomingPacketParser<T> Parser { get; }
        public AbstractIncomingClientSessionPacketHandler<T> Handler { get; }

        public IncomingClientSessionPacketConsumer(IIncomingPacketParser<T> parser, AbstractIncomingClientSessionPacketHandler<T> handler)
        {
            this.Parser = parser;
            this.Handler = handler;
        }

        public void Read(ClientSession session, ref PacketReader reader) => this.Handle(session, this.Parse(ref reader));

        public T Parse(ref PacketReader reader) => this.Parser.Parse(ref reader);

        internal override void Handle(ClientSession session, in T packet) => this.Handler.Handle(session, packet);
    }
}
