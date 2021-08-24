using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming
{
    internal class GetHatIncomingMessage : MessageIncomingJson<JsonGetHatIncomingMessage>
    {
        internal override void Handle(ClientSession session, JsonGetHatIncomingMessage message)
        {
            if (!session.IsLoggedIn)
            {
                return;
            }

            session.MultiplayerMatchSession?.MatchPlayer?.Match.GetHat(session, message.Id);
        }
    }
}
