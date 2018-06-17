using Platform_Racing_3_Common.Level;
using Platform_Racing_3_Common.User;
using Platform_Racing_3_Server.Collections;
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

        internal async Task<MatchListing> TryCreateMatchAsync(ClientSession session, uint levelId, uint version, uint minRank, uint maxRank, uint maxMembers, bool onlyFriends)
        {
            if (session.LobbySession.MatchListing == null)
            {
                LevelData level = await LevelManager.GetLevelDataAsync(levelId, version);
                if (level != null && level.Publish || (!session.IsGuest && level.AuthorUserId == session.UserData.Id) || session.HasPermissions(Permissions.ACCESS_SEE_UNPUBLISHED_LEVELS))
                {
                    if (maxMembers < 1)
                    {
                        maxMembers = 1;
                    }
                    else if (maxMembers > 8 && !session.HasPermissions(Permissions.ACCESS_MATCH_LISTING_NO_MEMBERS_LIMIT))
                    {
                        maxMembers = 8;
                    }

                    MatchListing listing = new MatchListing(session, level, "match-listing-" + this.GetNextMatchListingId(), minRank, maxRank, maxMembers, onlyFriends);
                    session.SendPacket(new MatchCreatedOutgoingMessage(listing));
                    listing.Join(session);
                    if (maxMembers == 1 || this.MatchListings.TryAdd(listing.Name, listing))
                    {
                        if (--maxMembers > 0) //Can we do qucick join?
                        {
                            foreach (ClientSession other in this.QuickJoinClients.Values)
                            {
                                if (maxMembers > 0) //Can we will it up more
                                {
                                    if (listing.CanJoin(other) == MatchListingJoinStatus.Success && this.QuickJoinClients.Remove(other))
                                    {
                                        other.SendPacket(new QuickJoinSuccessOutgoingMessage(listing));

                                        maxMembers--;
                                    }
                                }
                            }
                        }

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
            return this.MatchListings.Values.Except(session.LobbySession.Matches).Where((m) => m.CanJoin(session) == MatchListingJoinStatus.Success).OrderByDescending((l) => l.ClientsCount).Take((int)num).ToList();
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
                if (listing.CanJoin(session) == MatchListingJoinStatus.Success && this.QuickJoinClients.Remove(session))
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
