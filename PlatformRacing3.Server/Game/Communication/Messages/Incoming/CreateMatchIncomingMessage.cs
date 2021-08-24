using System.Threading.Tasks;
using PlatformRacing3.Common.Level;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing;
using PlatformRacing3.Server.Game.Lobby;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming
{
    internal sealed class CreateMatchIncomingMessage : MessageIncomingJson<JsonCreateMatchIncomingMessage>
    {
        private readonly MatchListingManager matchListingManager;

        public CreateMatchIncomingMessage(MatchListingManager matchListingManager)
        {
            this.matchListingManager = matchListingManager;
        }

        internal override void Handle(ClientSession session, JsonCreateMatchIncomingMessage message)
        {
            if (!session.IsLoggedIn)
            {
                return;
            }

            _ = CreateMatch();

            async Task CreateMatch()
            {
                MatchListing listing;
                if (message.Version == 0)
                {
                    LevelData level = await LevelManager.GetLevelDataAsync(message.LevelId);

                    listing = this.matchListingManager.TryCreateMatch(session, level, message.MinRank, message.MaxRank, message.MaxMembers, message.OnlyFriends, session.HostTournament ? MatchListingType.Tournament : MatchListingType.Normal);
                }
                else
                {
                    listing = await this.matchListingManager.TryCreateMatchAsync(session, message.LevelId, message.Version, message.MinRank, message.MaxRank, message.MaxMembers, message.OnlyFriends, session.HostTournament ? MatchListingType.Tournament : MatchListingType.Normal);
                }

                if (listing == null)
                {
                    session.SendPacket(new MatchFailedOutgoingMessage());
                }

                session.HostTournament = false;
            }
        }
    }
}
