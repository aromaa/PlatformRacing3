using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json.Converters
{
    internal class JsonPacketConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => typeof(JsonPacket).IsAssignableFrom(objectType);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string packetType = null;

            JObject item = JObject.Load(reader);
            if (item.TryGetValue("type", out JToken type))
            {
                packetType = (string)type;
            }
            else if (item.TryGetValue("t", out JToken t))
            {
                packetType = (string)t;
            }

            JsonPacket target = null;
            switch (packetType) //TODO: Umh...
            {
                case "ping":
                    target = new JsonLegacyPingIncomingMessage();
                    break;
                case "login":
                    target = new JsonTokenLoginIncomingMessage();
                    break;
                case "token_login":
                    target = new JsonTokenLoginIncomingMessage();
                    break;
                case "mv":
                    target = new JsonManageVarsIncomingMessage();
                    break;
                case "set_account_settings":
                    target = new JsonSetAccountSettingsMessage();
                    break;
                case "get_level_list":
                    target = new JsonGetLevelListIncomingMessage();
                    break;
                case "jr":
                    target = new JsonJoinRoomIncomingMessage();
                    break;
                case "lr":
                    target = new JsonLeaveRoomIncomingMessage();
                    break;
                case "get_member_list":
                    target = new JsonGetMemberListIncomingMessage();
                    break;
                case "get_user_list":
                    target = new JsonGetUserListIncomingMessage();
                    break;
                case "get_user_page":
                    target = new JsonGetUserPageIncomingMessage();
                    break;
                case "sr":
                    target = new JsonSendToRoomIncomingMessage();
                    break;
                case "create_match":
                    target = new JsonCreateMatchIncomingMessage();
                    break;
                case "request_matches":
                    target = new JsonRequestMatchesIncomingMessage();
                    break;
                case "lose_hat":
                    target = new JsonLoseHatIncomingMessage();
                    break;
                case "get_hat":
                    target = new JsonGetHatIncomingMessage();
                    break;
                case "coins":
                    target = new JsonCoinsIncomingMessage();
                    break;
                case "get_pms":
                    target = new JsonGetPmsIncomingMessage();
                    break;
                case "get_pm":
                    target = new JsonGetPmIncomingMessage();
                    break;
                case "delete_pms":
                    target = new JsonDeletePmsIncomingMessage();
                    break;
                case "send_pm":
                    target = new JsonSendPmIncomingMessage();
                    break;
                case "edit_user_list":
                    target = new JsonEditUserListIncomingMessage();
                    break;
                case "kick":
                    target = new JsonKickFromMatchListingIncomingMessage();
                    break;
                case "ban":
                    target = new JsonBanFromMatchListingIncomingMessage();
                    break;
                case "rate_level":
                    target = new JsonRateLevelIncomingMessage();
                    break;
                case "send_thing":
                    target = new JsonSendThingIncomingMessage();
                    break;
                case "thingExits":
                    target = new JsonThingExistsIncomingMessage();
                    break;
                case "accept_thing_transfer":
                    target = new JsonAcceptThingTransferIncomingMessage();
                    break;
                case "report_pm":
                    target = new JsonReportPmIncomingMessage();
                    break;
                case "koth":
                    target = new JsonKothIncomingMessage();
                    break;
                case "dash":
                    target = new JsonDashIncomingMessage();
                    break;
                case "win_hat":
                    target = new JsonWinHatIncomingMessage();
                    break;
                default:
                    target = new JsonPacket();
                    break;
            }

            serializer.Populate(item.CreateReader(), target);

            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotImplementedException();
    }
}
