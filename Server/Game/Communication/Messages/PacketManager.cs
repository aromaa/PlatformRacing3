using Platform_Racing_3_Server.Game.Communication.Messages.Incoming;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Platform_Racing_3_Server.Game.Lobby;
using Platform_Racing_3_Server.Game.Match;

namespace Platform_Racing_3_Server.Game.Communication.Messages
{
    internal sealed class PacketManager
    {
        private Dictionary<string, IMessageIncomingJson> IncomingPacketsJSON;

        public PacketManager(MatchListingManager matchListingManager, MatchManager matchManager, ILoggerFactory loggerFactory)
        {
            this.IncomingPacketsJSON = new Dictionary<string, IMessageIncomingJson>()
            {
                { "confirm_connection", new ConfirmConnectionIncomingMessage() },
                { "token_login", new TokenLoginIncomingMessage(loggerFactory.CreateLogger<TokenLoginIncomingMessage>()) },
                { "guest_login", new GuestLoginIncomingMessage(loggerFactory.CreateLogger<GuestLoginIncomingMessage>()) },
                { "ping", new LegacyPingIncomingMessage() },
                { "mv", new ManageVarsIncomingMessage() },
                { "set_account_settings", new SetAccountSettingsIncomingMessage() },
                { "get_level_list", new GetLevelListIncomingMessage() },
                { "jr", new JoinRoomIncomingMessage(matchListingManager, matchManager) },
                { "get_lotd", new GetLevelOfTheDayIncomingMessage(matchListingManager) },
                { "get_tournament", new GetTournamentIncomingMessage(matchListingManager, matchManager) },
                { "request_matches", new RequestMatchesIncominggMessage(matchListingManager) },
                { "gr", new GetRoomsIncomingMessage() },
                { "lr", new LeaveRoomIncomingMessage(matchListingManager, matchManager) },
                { "leave_lobby", new LeaveLobbyIncomingMessage() },
                { "get_member_list", new GetMemberListIncomingMessage() },
                { "get_user_list", new GetUserListIncomingMessage() },
                { "get_user_page", new GetUserPageIncomingMessage() },
                { "sr", new SendToRoomIncomingMessage() },
                { "create_match", new CreateMatchIncomingMessage(matchListingManager) },
                { "force_start", new ForceStartIncomingMessage() },
                { "finish_drawing", new FinishDrawingIncomingMessage() },
                { "forfiet", new ForfietIncomingMessage() },
                { "finish_match", new FinishMatchIncomingMessage() },
                { "lose_hat", new LoseHatIncomingMessage() },
                { "get_hat", new GetHatIncomingMessage() },
                { "coins", new CoinsIncomingMessage() },
                { "get_pms", new GetPmsIncomingMessage() },
                { "get_pm", new GetPmIncomingMessage() },
                { "delete_pms", new DeletePmsIncomingMessage() },
                { "send_pm", new SendPmIncomingMessage() },
                { "edit_user_list", new EditUserListIncomingMessage() },
                { "kick", new KickFromMatchListingIncomingMessage() },
                { "ban", new BanFromMatchListingIncomingMessage() },
                { "start_quick_join", new StartQucikJoinIncomingMessage(matchListingManager) },
                { "stop_quick_join", new StopQuickJoinIncomingMessage(matchListingManager) },
                { "rate_level", new RateLevelIncomingMessage() },
                { "send_thing", new SendThingIncomingMessage() },
                { "thingExits", new ThingExistsIncomingMessage() },
                { "accept_thing_transfer", new AcceptThingTransferIncomingMessage() },
                { "report_pm", new ReportPmIncomingMessage() },
                { "koth", new KothIncomingMessage() },
				{ "dash", new DashIncomingMessage() },
                { "join_tournament", new JoinTournamentIncomingMessage(matchListingManager, matchManager) },
                { "win_hat", new WinHatIncomingMessage() },
            };
        }

        internal bool GetIncomingJSONPacket(string packetId, out IMessageIncomingJson handler)
        {
            return this.IncomingPacketsJSON.TryGetValue(packetId, out handler);
        }
    }
}
