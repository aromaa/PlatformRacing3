using Net.Communication.Attributes;
using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Managers;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Handlers.Json
{
    [PacketManagerRegister(typeof(BytePacketManager))]
    internal sealed class JsonPacketHandler : AbstractIncomingClientSessionPacketHandler<JsonPacket>
    {
        private readonly PacketManager packetManager;

        public JsonPacketHandler(PacketManager packetManager)
        {
            this.packetManager = packetManager;
        }

        internal override void Handle(ClientSession session, in JsonPacket packet)
        {
            if (this.packetManager.GetIncomingJSONPacket(packet.GetType(), out IMessageIncomingJson handler))
            {
                handler.Handle(session, packet);
            }
        }
    }
}
