using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages;
using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Text;
using Net.Buffers;
using Net.Sockets.Pipeline.Handler;
using Net.Sockets.Pipeline.Handler.Incoming;
using Net.Sockets.Pipeline.Handler.Outgoing;
using Platform_Racing_3_Server.Game.Communication.Managers;

namespace Platform_Racing_3_Server.Game.Communication.Handlers
{
    internal sealed class SplitPacketHandler : IncomingBytesHandler, IOutgoingObjectHandler
    {
        private readonly BytePacketManager bytePacketManager;

        private ushort CurrentPacketLength;

        private ClientSession Session;

        internal SplitPacketHandler(BytePacketManager bytePacketManager, ClientSession session)
        {
            this.bytePacketManager = bytePacketManager;

            this.Session = session;
        }

        protected override void Decode(IPipelineHandlerContext context, ref PacketReader reader)
        {
            //We haven't read the next packet length, wait for it
            if (this.CurrentPacketLength == 0 && !reader.TryReadUInt16(out this.CurrentPacketLength))
            {
                return;
            }

            if (reader.Remaining < this.CurrentPacketLength)
            {
                return;
            }

            PacketReader readerSliced = reader.Slice(this.CurrentPacketLength);

            this.Read(context, ref readerSliced);

            this.CurrentPacketLength = 0;
        }

        public void Read(IPipelineHandlerContext context, ref PacketReader reader)
        {
            ushort header = reader.ReadUInt16();

            this.bytePacketManager.HandleIncomingData(header, this.Session, context, ref reader);

            if (reader.Remaining > 0)
            {
                reader.Skip(reader.Remaining); //Skip extra bytes and head to the next packet
            }
        }

        public void Handle<T>(IPipelineHandlerContext context, ref PacketWriter writer, in T packet)
        {
            if (packet is IMessageOutgoing message)
            {
                PacketWriter length = writer.ReservedFixedSlice(2);

                int writerLength = writer.Length;

                message.Write(ref writer);

                ushort size = checked((ushort)(writer.Length - writerLength));

                length.WriteUInt16(size);
            }
        }
    }
}
