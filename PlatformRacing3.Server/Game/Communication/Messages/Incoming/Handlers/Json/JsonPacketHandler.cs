using Net.Communication.Attributes;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Managers;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Handlers.Json
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
