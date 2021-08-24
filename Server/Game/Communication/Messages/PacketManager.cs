using Platform_Racing_3_Server.Game.Communication.Messages.Incoming;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Platform_Racing_3_Common.Server;
using Platform_Racing_3_Server.Game.Chat;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Lobby;
using Platform_Racing_3_Server.Game.Match;

namespace Platform_Racing_3_Server.Game.Communication.Messages
{
    internal sealed class PacketManager
    {
        private Dictionary<Type, IMessageIncomingJson> IncomingPacketsJSON;

        public PacketManager(ServerManager serverManager, ClientManager clientManager, ChatRoomManager chatRoomManager, MatchListingManager matchListingManager, MatchManager matchManager, ILoggerFactory loggerFactory)
        {
            this.IncomingPacketsJSON = new Dictionary<Type, IMessageIncomingJson>()
            {
                { typeof(JsonConfirmConnectionIncomingMessage), new ConfirmConnectionIncomingMessage() },
                { typeof(JsonTokenLoginIncomingMessage), new TokenLoginIncomingMessage(serverManager, clientManager, loggerFactory.CreateLogger<TokenLoginIncomingMessage>()) },
                { typeof(JsonGuestLoginIncomingPacket), new GuestLoginIncomingMessage(serverManager, clientManager, loggerFactory.CreateLogger<GuestLoginIncomingMessage>()) },
                { typeof(JsonLegacyPingIncomingMessage), new LegacyPingIncomingMessage() },
                { typeof(JsonManageVarsIncomingMessage), new ManageVarsIncomingMessage() },
                { typeof(JsonSetAccountSettingsMessage), new SetAccountSettingsIncomingMessage() },
                { typeof(JsonGetLevelListIncomingMessage), new GetLevelListIncomingMessage() },
                { typeof(JsonJoinRoomIncomingMessage), new JoinRoomIncomingMessage(chatRoomManager, matchListingManager, matchManager) },
                { typeof(JsonGetLevelOfTheDayIncomingMessage), new GetLevelOfTheDayIncomingMessage(matchListingManager) },
                { typeof(JsonGetTournamentIncomingMessage), new GetTournamentIncomingMessage(matchListingManager, matchManager) },
                { typeof(JsonRequestMatchesIncomingMessage), new RequestMatchesIncominggMessage(matchListingManager) },
                { typeof(JsonGetRoomsIncomingMessage), new GetRoomsIncomingMessage(chatRoomManager) },
                { typeof(JsonLeaveRoomIncomingMessage), new LeaveRoomIncomingMessage(chatRoomManager, matchListingManager, matchManager) },
                { typeof(JsonLeaveLobbyIncomingMessage), new LeaveLobbyIncomingMessage() },
                { typeof(JsonGetMemberListIncomingMessage), new GetMemberListIncomingMessage(chatRoomManager) },
                { typeof(JsonGetUserListIncomingMessage), new GetUserListIncomingMessage(clientManager) },
                { typeof(JsonGetUserPageIncomingMessage), new GetUserPageIncomingMessage(clientManager) },
                { typeof(JsonSendToRoomIncomingMessage), new SendToRoomIncomingMessage(chatRoomManager) },
                { typeof(JsonCreateMatchIncomingMessage), new CreateMatchIncomingMessage(matchListingManager) },
                { typeof(JsonForceStartIncomingMessage), new ForceStartIncomingMessage() },
                { typeof(JsonFinishDrawingIncomingMessage), new FinishDrawingIncomingMessage() },
                { typeof(JsonForfietIncomingMessage), new ForfietIncomingMessage() },
                { typeof(JsonFinishMatchIncomingMessage), new FinishMatchIncomingMessage() },
                { typeof(JsonLoseHatIncomingMessage), new LoseHatIncomingMessage() },
                { typeof(JsonGetHatIncomingMessage), new GetHatIncomingMessage() },
                { typeof(JsonCoinsIncomingMessage), new CoinsIncomingMessage() },
                { typeof(JsonGetPmsIncomingMessage), new GetPmsIncomingMessage() },
                { typeof(JsonGetPmIncomingMessage), new GetPmIncomingMessage() },
                { typeof(JsonDeletePmsIncomingMessage), new DeletePmsIncomingMessage() },
                { typeof(JsonSendPmIncomingMessage), new SendPmIncomingMessage() },
                { typeof(JsonEditUserListIncomingMessage), new EditUserListIncomingMessage() },
                { typeof(JsonKickFromMatchListingIncomingMessage), new KickFromMatchListingIncomingMessage() },
                { typeof(JsonBanFromMatchListingIncomingMessage), new BanFromMatchListingIncomingMessage() },
                { typeof(JsonStartQucikJoinIncomingMessage), new StartQucikJoinIncomingMessage(matchListingManager) },
                { typeof(JsonStopQuickJoinIncomingMessage), new StopQuickJoinIncomingMessage(matchListingManager) },
                { typeof(JsonRateLevelIncomingMessage), new RateLevelIncomingMessage() },
                { typeof(JsonSendThingIncomingMessage), new SendThingIncomingMessage() },
                { typeof(JsonThingExistsIncomingMessage), new ThingExistsIncomingMessage() },
                { typeof(JsonAcceptThingTransferIncomingMessage), new AcceptThingTransferIncomingMessage() },
                { typeof(JsonReportPmIncomingMessage), new ReportPmIncomingMessage() },
                { typeof(JsonKothIncomingMessage), new KothIncomingMessage() },
				{ typeof(JsonDashIncomingMessage), new DashIncomingMessage() },
                { typeof(JsonJoinTournamentIncomingMessage), new JoinTournamentIncomingMessage(matchListingManager, matchManager) },
                { typeof(JsonWinHatIncomingMessage), new WinHatIncomingMessage() },
            };
        }

        internal bool GetIncomingJSONPacket(Type packetId, out IMessageIncomingJson handler)
        {
            return this.IncomingPacketsJSON.TryGetValue(packetId, out handler);
        }
    }
}
