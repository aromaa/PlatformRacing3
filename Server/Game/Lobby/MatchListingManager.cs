using Platform_Racing_3_Common.Level;
using Platform_Racing_3_Common.User;
using Platform_Racing_3_Server.Collections;
using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Extensions;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Platform_Racing_3_Server.Game.Match;

namespace Platform_Racing_3_Server.Game.Lobby
{
    internal sealed class MatchListingManager
    {
        private readonly MatchManager matchManager;

        private int NextMatchListingId;
        private ConcurrentDictionary<string, MatchListing> MatchListings;

        private ClientSessionCollection QuickJoinClients;

        public MatchListingManager(MatchManager matchManager)
        {
            this.matchManager = matchManager;

            this.MatchListings = new ConcurrentDictionary<string, MatchListing>();

            this.QuickJoinClients = new ClientSessionCollection();
        }

        private uint GetNextMatchListingId() => (uint)Interlocked.Increment(ref this.NextMatchListingId);

        internal MatchListing TryCreateMatch(ClientSession session, LevelData level, uint minRank, uint maxRank, uint maxMembers, bool onlyFriends, MatchListingType type = MatchListingType.Normal)
        {
            if (session.LobbySession.MatchListing == null)
            {
                if (level != null && (level.Publish || (!session.IsGuest && level.AuthorUserId == session.UserData.Id) || session.HasPermissions(Permissions.ACCESS_SEE_UNPUBLISHED_LEVELS)))
                {
                    if (maxMembers < 1)
                    {
                        maxMembers = 1;
                    }
                    //else if (maxMembers > 8 && !session.HasPermissions(Permissions.ACCESS_MATCH_LISTING_NO_MEMBERS_LIMIT))
                    //{
                    //    maxMembers = 8;
                    //}

                    MatchListing listing = new(this, this.matchManager, type, session, level, type.GetLobbyId(this.GetNextMatchListingId()), minRank, maxRank, maxMembers, onlyFriends);
                    session.SendPacket(new MatchCreatedOutgoingMessage(listing));

                    if (this.MatchListings.TryAdd(listing.Name, listing))
                    {
                        return listing;
                    }
                    else
                    {
                        listing.Leave(session);
                    }
                }
            }

            return null;
        }

        internal async Task<MatchListing> TryCreateMatchAsync(ClientSession session, uint levelId, uint version, uint minRank, uint maxRank, uint maxMembers, bool onlyFriends, MatchListingType type = MatchListingType.Normal)
        {
            LevelData level = await LevelManager.GetLevelDataAsync(levelId, version);

            return this.TryCreateMatch(session, level, minRank, maxRank, maxMembers, onlyFriends, type);
        }

        internal MatchListing Join(ClientSession session, string roomName, out bool status)
        {
            status = false;
            
            if (this.MatchListings.TryGetValue(roomName, out MatchListing matchListing))
            {
                if (session.LobbySession.MatchListing == null)
                {
                    status = matchListing.Join(session);
                    if (status)
                    {
                        //Do quick join here bcs the client is a big mess
                        foreach (ClientSession other in this.QuickJoinClients.Sessions)
                        {
                            if (matchListing.CanJoin(other) == MatchListingJoinStatus.Success)
                            {
                                if (!this.QuickJoinClients.TryRemove(session))
                                {
                                    continue;
                                }

                                other.SendPacket(new QuickJoinSuccessOutgoingMessage(matchListing));
                            }
                            else
                            {
                                break;
                            }
                        }

                        return matchListing;
                    }
                }
                else if (session.LobbySession.MatchListing == matchListing)
                {
                    status = true;

                    return matchListing;
                }
            }

            return null;
        }

        internal void Leave(ClientSession session, string roomName)
        {
            if (this.MatchListings.TryGetValue(roomName, out MatchListing matchListing) && session.LobbySession.MatchListing == matchListing)
            {
                matchListing.Leave(session);
            }
        }

        internal List<MatchListing> RequestsMatches(ClientSession session, uint num)
        {
            return this.MatchListings.Values.Except(session.LobbySession.Matches).Where((m) => m.Type == MatchListingType.Normal && m.CanJoin(session) == MatchListingJoinStatus.Success).OrderByDescending((l) => l.ClientsCount).Take((int)num).ToList();
        }

        internal MatchListing GetTournament(ClientSession session)
        {
            return this.MatchListings.Values.Where((m) => m.Type == MatchListingType.Tournament && m.CanJoin(session) == MatchListingJoinStatus.Success).OrderByDescending((l) => l.ClientsCount).FirstOrDefault();
        }

        internal MatchListing GetLeveOfTheDay()
        {
            MatchListing listing = this.MatchListings.Values.Where((m) => m.Type == MatchListingType.LevelOfTheDay).OrderByDescending((l) => l.ClientsCount).FirstOrDefault();
            if (listing == null)
            {
                IReadOnlyCollection<LevelData> levels = LevelManager.GetCampaignLevels().GetAwaiter().GetResult().Levels;
                if (levels.Count > 0)
                {
                    LevelData random = levels.ElementAt(new Random().Next(0, levels.Count));

                    listing = new MatchListing(this, this.matchManager, MatchListingType.LevelOfTheDay, null, random, MatchListingType.LevelOfTheDay.GetLobbyId(this.GetNextMatchListingId()), 0, uint.MaxValue, 4, false);

                    if (!this.MatchListings.TryAdd(listing.Name, listing))
                    {
                        return null;
                    }
                }
            }

            return listing;
        }

        internal void Die(MatchListing matchListing)
        {
            this.MatchListings.TryRemove(matchListing.Name, out _);
        }

        internal void StartQuickJoin(ClientSession session)
        {
            this.QuickJoinClients.TryAdd(session);

            foreach(MatchListing listing in this.MatchListings.Values)
            {
                if (listing.Type == MatchListingType.Normal && listing.CanJoin(session) == MatchListingJoinStatus.Success && this.QuickJoinClients.TryRemove(session))
                {
                    session.SendPacket(new QuickJoinSuccessOutgoingMessage(listing));
                    break;
                }
            }
        }

        internal void StopQuickJoin(ClientSession session)
        {
            this.QuickJoinClients.TryRemove(session);
        }
    }
}
