using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming
{
    internal class CoinsIncomingMessage : MessageIncomingJson<JsonCoinsIncomingMessage>
    {
        internal override void Handle(ClientSession session, JsonCoinsIncomingMessage message)
        {
            if (!session.IsLoggedIn)
            {
                return;
            }

            session.MultiplayerMatchSession?.MatchPlayer?.Match.UpdateCoins(session, message.Coins);
        }
    }
}
