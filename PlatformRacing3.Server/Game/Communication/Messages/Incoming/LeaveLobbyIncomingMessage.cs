using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming
{
    internal class LeaveLobbyIncomingMessage : IMessageIncomingJson
    {
        public void Handle(ClientSession session, JsonPacket message)
        {
            session.LobbySession?.LeaveLobby();
        }
    }
}
