using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing;
using Platform_Racing_3_Server.Game.Lobby;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Platform_Racing_3_Common.Level;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class CreateMatchIncomingMessage : MessageIncomingJson<JsonCreateMatchIncomingMessage>
    {
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

                    listing = PlatformRacing3Server.MatchListingManager.TryCreateMatch(session, level, message.MinRank, message.MaxRank, message.MaxMembers, message.OnlyFriends, session.HostTournament ? MatchListingType.Tournament : MatchListingType.Normal);
                }
                else
                {
                    listing = await PlatformRacing3Server.MatchListingManager.TryCreateMatchAsync(session, message.LevelId, message.Version, message.MinRank, message.MaxRank, message.MaxMembers, message.OnlyFriends, session.HostTournament ? MatchListingType.Tournament : MatchListingType.Normal);
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
