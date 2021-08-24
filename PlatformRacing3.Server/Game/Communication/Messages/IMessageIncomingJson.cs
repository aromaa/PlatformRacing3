using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages
{
    internal interface IMessageIncomingJson
    {
        void Handle(ClientSession session, JsonPacket message);
    }
}
