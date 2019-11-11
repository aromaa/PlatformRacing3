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

namespace Platform_Racing_3_Server.Game.Lobby
{
    internal class MatchListingManager
    {
        private int NextMatchListingId;
        private ConcurrentDictionary<string, MatchListing> MatchListings;

        private ClientSessionCollection QuickJoinClients;

        internal MatchListingManager()
        {
            this.MatchListings = new ConcurrentDictionary<string, MatchListing>();

            this.QuickJoinClients = new ClientSessionCollection();
        }

        private uint GetNextMatchListingId() => (uint)Interlocked.Increment(ref this.NextMatchListingId);

        internal Task<MatchListing> TryCreateMatchAsync(ClientSession session, LevelData levelData, uint minRank, uint maxRank, uint maxMembers, bool onlyFriends, MatchListingType type = MatchListingType.Normal)
        {
            return this.TryCreateMatchAsync(session, levelData.Id, levelData.Version, minRank, maxRank, maxMembers, onlyFriends, type);
        }

        internal async Task<MatchListing> TryCreateMatchAsync(ClientSession session, uint levelId, uint version, uint minRank, uint maxRank, uint maxMembers, bool onlyFriends, MatchListingType type = MatchListingType.Normal)
        {
            if (session.LobbySession.MatchListing == null)
            {
                LevelData level = await LevelManager.GetLevelDataAsync(levelId, version);
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

                    MatchListing listing = new MatchListing(type, session, level, type.GetLobbyId(this.GetNextMatchListingId()), minRank, maxRank, maxMembers, onlyFriends);
                    session.SendPacket(new MatchCreatedOutgoingMessage(listing));

                    if (this.MatchListings.TryAdd(listing.Name, listing))
                    {
                        return listing;
                    }
                    else
                    {
                        listing.Leave(session, MatchListingLeaveReason.FailedJoin);
                    }
                }
            }

            return null;
        }

        internal MatchListing Join(ClientSession session, string roomName, out MatchListingJoinStatus status)
        {
            status = MatchListingJoinStatus.Failed;
            
            if (this.MatchListings.TryGetValue(roomName, out MatchListing matchListing))
            {
                if (session.LobbySession.MatchListing == null)
                {
                    status = matchListing.Join(session);
                    if (status == MatchListingJoinStatus.Success)
                    {
                        //Do quick join here bcs the client is a big mess
                        uint spotsLeft = matchListing.SpotsLeft;
                        foreach (ClientSession other in this.QuickJoinClients.Values)
                        {
                            if (spotsLeft > 0) //Can we fill it up even more
                            {
                                if (matchListing.CanJoin(other) == MatchListingJoinStatus.Success && this.QuickJoinClients.Remove(other))
                                {
                                    other.SendPacket(new QuickJoinSuccessOutgoingMessage(matchListing));

                                    spotsLeft--;
                                }
                            }
                        }

                        return matchListing;
                    }
                }
                else if (session.LobbySession.MatchListing == matchListing)
                {
                    status = MatchListingJoinStatus.Success;

                    return matchListing;
                }
            }

            return null;
        }

        internal void Leave(ClientSession session, string roomName)
        {
            if (this.MatchListings.TryGetValue(roomName, out MatchListing matchListing) && session.LobbySession.MatchListing == matchListing)
            {
                matchListing.Leave(session, MatchListingLeaveReason.Quit);
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

                    listing = new MatchListing(MatchListingType.LevelOfTheDay, null, random, MatchListingType.LevelOfTheDay.GetLobbyId(this.GetNextMatchListingId()), 0, uint.MaxValue, 4, false);

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
            this.QuickJoinClients.Add(session);

            foreach(MatchListing listing in this.MatchListings.Values)
            {
                if (listing.Type == MatchListingType.Normal && listing.CanJoin(session) == MatchListingJoinStatus.Success && this.QuickJoinClients.Remove(session))
                {
                    session.SendPacket(new QuickJoinSuccessOutgoingMessage(listing));
                    break;
                }
            }
        }

        internal void StopQuickJoin(ClientSession session)
        {
            this.QuickJoinClients.Remove(session);
        }
    }
}
