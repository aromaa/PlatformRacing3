using Platform_Racing_3_Server.Game.Communication.Messages.Incoming;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages
{
    internal class PacketManager
    {
        private Dictionary<string, IMessageIncomingJson> IncomingPacketsJSON;

        internal PacketManager()
        {
            this.IncomingPacketsJSON = new Dictionary<string, IMessageIncomingJson>()
            {
                { "confirm_connection", new ConfirmConnectionIncomingMessage() },
                { "token_login", new TokenLoginIncomingMessage() },
                { "guest_login", new GuestLoginIncomingMessage() },
                { "ping", new LegacyPingIncomingMessage() },
                { "mv", new ManageVarsIncomingMessage() },
                { "set_account_settings", new SetAccountSettingsIncomingMessage() },
                { "get_level_list", new GetLevelListIncomingMessage() },
                { "jr", new JoinRoomIncomingMessage() },
                { "get_lotd", new GetLevelOfTheDayIncomingMessage() },
                { "get_tournament", new GetTournamentIncomingMessage() },
                { "request_matches", new RequestMatchesIncominggMessage() },
                { "gr", new GetRoomsIncomingMessage() },
                { "lr", new LeaveRoomIncomingMessage() },
                { "leave_lobby", new LeaveLobbyIncomingMessage() },
                { "get_member_list", new GetMemberListIncomingMessage() },
                { "get_user_list", new GetUserListIncomingMessage() },
                { "get_user_page", new GetUserPageIncomingMessage() },
                { "sr", new SendToRoomIncomingMessage() },
                { "create_match", new CreateMatchIncomingMessage() },
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
                { "start_quick_join", new StartQucikJoinIncomingMessage() },
                { "stop_quick_join", new StopQuickJoinIncomingMessage() },
                { "rate_level", new RateLevelIncomingMessage() },
                { "send_thing", new SendThingIncomingMessage() },
                { "thingExits", new ThingExistsIncomingMessage() },
                { "accept_thing_transfer", new AcceptThingTransferIncomingMessage() },
                { "report_pm", new ReportPmIncomingMessage() },
                { "koth", new KothIncomingMessage() },
				{ "dash", new DashIncomingMessage() },
                { "join_tournament", new JoinTournamentIncomingMessage() },
                { "win_hat", new WinHatIncomingMessage() },
            };
        }

        internal bool GetIncomingJSONPacket(string packetId, out IMessageIncomingJson handler)
        {
            return this.IncomingPacketsJSON.TryGetValue(packetId, out handler);
        }
    }
}
