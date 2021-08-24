using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming
{
    internal class LoseHatIncomingMessage : MessageIncomingJson<JsonLoseHatIncomingMessage>
    {
        internal override void Handle(ClientSession session, JsonLoseHatIncomingMessage message)
        {
            if (!session.IsLoggedIn)
            {
                return;
            }

            session.MultiplayerMatchSession?.MatchPlayer?.Match.LoseHat(session, message.X, message.Y, message.VelX, message.VelY);
        }
    }
}
