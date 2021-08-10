using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json.Converters
{
	internal sealed class JsonPacketConverter : JsonConverter<JsonPacket>
	{
		private static readonly Dictionary<string, Type> packets = new()
		{
			{ "ping", typeof(JsonLegacyPingIncomingMessage) },
			{ "token_login", typeof(JsonTokenLoginIncomingMessage) },
			{ "mv", typeof(JsonManageVarsIncomingMessage) },
			{ "set_account_settings", typeof(JsonSetAccountSettingsMessage) },
			{ "get_level_list", typeof(JsonGetLevelListIncomingMessage) },
			{ "jr", typeof(JsonJoinRoomIncomingMessage) },
			{ "lr", typeof(JsonLeaveRoomIncomingMessage) },
			{ "get_member_list", typeof(JsonGetMemberListIncomingMessage) },
			{ "get_user_list", typeof(JsonGetUserListIncomingMessage) },
			{ "get_user_page", typeof(JsonGetUserPageIncomingMessage) },
			{ "sr", typeof(JsonSendToRoomIncomingMessage) },
			{ "create_match", typeof(JsonCreateMatchIncomingMessage) },
			{ "request_matches", typeof(JsonRequestMatchesIncomingMessage) },
			{ "lose_hat", typeof(JsonLoseHatIncomingMessage) },
			{ "get_hat", typeof(JsonGetHatIncomingMessage) },
			{ "coins", typeof(JsonCoinsIncomingMessage) },
			{ "get_pms", typeof(JsonGetPmsIncomingMessage) },
			{ "get_pm", typeof(JsonGetPmIncomingMessage) },
			{ "delete_pms", typeof(JsonDeletePmsIncomingMessage) },
			{ "send_pm", typeof(JsonSendPmIncomingMessage) },
			{ "edit_user_list", typeof(JsonEditUserListIncomingMessage) },
			{ "kick", typeof(JsonKickFromMatchListingIncomingMessage) },
			{ "ban", typeof(JsonBanFromMatchListingIncomingMessage) },
			{ "rate_level", typeof(JsonRateLevelIncomingMessage) },
			{ "send_thing", typeof(JsonSendThingIncomingMessage) },
			{ "thingExits", typeof(JsonThingExistsIncomingMessage) },
			{ "accept_thing_transfer", typeof(JsonAcceptThingTransferIncomingMessage) },
			{ "report_pm", typeof(JsonReportPmIncomingMessage) },
			{ "koth", typeof(JsonKothIncomingMessage) },
			{ "dash", typeof(JsonDashIncomingMessage) },
			{ "win_hat", typeof(JsonWinHatIncomingMessage) }
		};
		
		public override JsonPacket Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			//Work on copy of the data so we can deserialize later on
			Utf8JsonReader documentReader = reader;
			if (!JsonDocument.TryParseValue(ref documentReader, out JsonDocument document))
			{
				throw new JsonException("Invalid document");
			}

			string type;
			using(document)
			{
				if (!document.RootElement.TryGetProperty("t", out JsonElement typeProperty))
				{
					if (!document.RootElement.TryGetProperty("type", out typeProperty))
					{
						throw new JsonException("Failed to parse packet identifier");
					}
				}

				type = typeProperty.GetString();
			}

			if (!JsonPacketConverter.packets.TryGetValue(type, out Type packetType))
			{
				packetType = typeof(JsonPacketNoData);
			}

			return Unsafe.As<JsonPacket>(JsonSerializer.Deserialize(ref reader, packetType, options));
		}

		public override void Write(Utf8JsonWriter writer, JsonPacket value, JsonSerializerOptions options) => throw new NotImplementedException();
	}
}
