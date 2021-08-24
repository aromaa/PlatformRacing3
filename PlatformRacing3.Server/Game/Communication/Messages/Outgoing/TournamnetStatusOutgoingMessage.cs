using PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing
{
    internal class TournamnetStatusOutgoingMessage : JsonOutgoingMessage<JsonTournamentStatusOutgoingMessage>
    {
        internal TournamnetStatusOutgoingMessage(byte status) : base(new JsonTournamentStatusOutgoingMessage(status))
        {
        }
    }
}
