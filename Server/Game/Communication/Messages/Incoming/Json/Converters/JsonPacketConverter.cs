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
			
            JsonPacket target = packetType switch //TODO: Umh...
			{
				"ping" => new JsonLegacyPingIncomingMessage(),
				"login" => new JsonTokenLoginIncomingMessage(),
				"token_login" => new JsonTokenLoginIncomingMessage(),
				"mv" => new JsonManageVarsIncomingMessage(),
				"set_account_settings" => new JsonSetAccountSettingsMessage(),
				"get_level_list" => new JsonGetLevelListIncomingMessage(),
				"jr" => new JsonJoinRoomIncomingMessage(),
				"lr" => new JsonLeaveRoomIncomingMessage(),
				"get_member_list" => new JsonGetMemberListIncomingMessage(),
				"get_user_list" => new JsonGetUserListIncomingMessage(),
				"get_user_page" => new JsonGetUserPageIncomingMessage(),
				"sr" => new JsonSendToRoomIncomingMessage(),
				"create_match" => new JsonCreateMatchIncomingMessage(),
				"request_matches" => new JsonRequestMatchesIncomingMessage(),
				"lose_hat" => new JsonLoseHatIncomingMessage(),
				"get_hat" => new JsonGetHatIncomingMessage(),
				"coins" => new JsonCoinsIncomingMessage(),
				"get_pms" => new JsonGetPmsIncomingMessage(),
				"get_pm" => new JsonGetPmIncomingMessage(),
				"delete_pms" => new JsonDeletePmsIncomingMessage(),
				"send_pm" => new JsonSendPmIncomingMessage(),
				"edit_user_list" => new JsonEditUserListIncomingMessage(),
				"kick" => new JsonKickFromMatchListingIncomingMessage(),
				"ban" => new JsonBanFromMatchListingIncomingMessage(),
				"rate_level" => new JsonRateLevelIncomingMessage(),
				"send_thing" => new JsonSendThingIncomingMessage(),
				"thingExits" => new JsonThingExistsIncomingMessage(),
				"accept_thing_transfer" => new JsonAcceptThingTransferIncomingMessage(),
				"report_pm" => new JsonReportPmIncomingMessage(),
				"koth" => new JsonKothIncomingMessage(),
				"dash" => new JsonDashIncomingMessage(),
				"win_hat" => new JsonWinHatIncomingMessage(),
				_ => new JsonPacket(),
			};

			serializer.Populate(item.CreateReader(), target);

            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotImplementedException();
    }
}
