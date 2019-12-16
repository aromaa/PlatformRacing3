using Net.Communication.Incoming.Handlers;
using Net.Communication.Incoming.Helpers;
using Net.Communication.Outgoing.Handlers;
using Net.Communication.Outgoing.Helpers;
using Net.Communication.Pipeline;
using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages;
using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Handlers
{
    internal class SplitPacketHandler : IncomingBytesHandler, IOutgoingObjectHandler
    {
        private ushort CurrentPacketLength;

        private ClientSession Session;

        internal SplitPacketHandler(ClientSession session)
        {
            this.Session = session;
        }

        public override void Handle(ref SocketPipelineContext context, ref PacketReader reader)
        {
            //We haven't read the next packet length, wait for it
            if (this.CurrentPacketLength == 0)
            {
                if (!reader.TryReadUInt16(out this.CurrentPacketLength))
                {
                    return;
                }

                reader.Consumed = true;
            }

            if (reader.Remaining < this.CurrentPacketLength)
            {
                return;
            }

            reader = reader.Slice(this.CurrentPacketLength);
            reader.Consumed = true;

            this.Read(ref context, ref reader);

            this.CurrentPacketLength = 0;
        }

        public void Read(ref SocketPipelineContext context, ref PacketReader reader)
        {
            ushort header = reader.ReadUInt16();

            if (PlatformRacing3Server.PacketManager.GetIncomingBytePacket(header, out IMessageIncomingBytes handler))
            {
                handler.Handle(this.Session, ref reader);
            }

            if (reader.Remaining > 0)
            {
                reader.Skip(reader.Remaining); //Skip extra bytes and head to the next packet
            }
        }

        public void Handle<T>(ref SocketPipelineContext context, in T data, ref PacketWriter writer)
        {
            if (data is IMessageOutgoing message)
            {
                PacketWriter.Slice length = writer.PrepareBytes(2);

                int writerLength = writer.Length;

                message.Write(ref writer);

                ushort size = (ushort)(writer.Length - writerLength);

                BinaryPrimitives.WriteUInt16BigEndian(length.Span, size);
            }
        }
    }
}
