using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Handlers;
using System;
using System.Collections.Generic;
using System.Text;
using Net.Buffers;
using Net.Communication.Incoming.Parser;
using Net.Sockets.Pipeline.Handler;

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
