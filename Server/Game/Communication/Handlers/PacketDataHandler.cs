using log4net;
using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages;
using Platform_Racing_3_Server.Net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Handlers
{
    internal class PacketDataHandler : IDataHandler<INetworkConnectionGame>
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const uint LENGTH_PACKET_SIZE = 2;

        private Queue<byte> UnhandledData;
        private int? UnhandledDataPacketLength;

        private ClientSession ClientSession;

        internal PacketDataHandler(INetworkConnectionGame connection)
        {
            this.UnhandledData = new Queue<byte>();

            this.ClientSession = new ClientSession(connection);
        }

        public void HandleData(INetworkConnectionGame connection, Span<byte> data)
        {
            for(int i = 0; i < data.Length; i++)
            {
                this.UnhandledData.Enqueue(data[i]);
            }

            while (this.UnhandledData.Count > 0)
            {
                if (this.UnhandledDataPacketLength == null && this.UnhandledData.Count >= PacketDataHandler.LENGTH_PACKET_SIZE)
                {
                    this.UnhandledDataPacketLength = (this.UnhandledData.Dequeue() << 8 | this.UnhandledData.Dequeue());
                }
                else if (this.UnhandledData.Count >= this.UnhandledDataPacketLength)
                {
                    byte[] data_ = new byte[(int)this.UnhandledDataPacketLength];
                    for(int i = 0; i < this.UnhandledDataPacketLength; i++)
                    {
                        data_[i] = this.UnhandledData.Dequeue();
                    }

                    this.HandleData(new IncomingMessage(data_));
                    
                    this.UnhandledDataPacketLength = null;
                }
                else
                {
                    break;
                }
            }
        }

        private void HandleData(IncomingMessage message)
        {
            ushort packetId = message.ReadUShort();
            if (PlatformRacing3Server.NetworkManager.PacketManager.GetIncomingBytePacket(packetId, out IMessageIncomingBytes handler))
            {
                handler.Handle(this.ClientSession, message);
            }
            else
            {
                PacketDataHandler.Logger.Info("Unhandle packet id: " + packetId);
            }
        }
    }
}
