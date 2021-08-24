using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;
using PlatformRacing3.Server.Game.Lobby;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming
{
    internal sealed class StopQuickJoinIncomingMessage : IMessageIncomingJson
    {
        private readonly MatchListingManager matchListingManager;

        public StopQuickJoinIncomingMessage(MatchListingManager matchListingManager)
        {
            this.matchListingManager = matchListingManager;
        }

        public void Handle(ClientSession session, JsonPacket message)
        {
            if (session.IsLoggedIn)
            {
                return;
            }

            this.matchListingManager.StopQuickJoin(session);
        }
    }
}
