using Net.Communication.Attributes;
using System.Text.Json.Serialization;
using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Communication.Managers;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Net.Buffers;
using Net.Communication.Incoming.Parser;
using Platform_Racing_3_Common.Json;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Parsers.Json
{
    [PacketManagerRegister(typeof(BytePacketManager))]
    [PacketParserId(0u)]
    internal sealed class JsonPacketParser : IIncomingPacketParser<JsonPacket>
    {
		private static readonly JsonIncomingPacketContext jsonContext = new(new JsonSerializerOptions
		{
			Converters =
			{
				new JsonColorConverter(),
				new JsonLevelModeConverter(),
				new JsonDateTimeConverter()
			}
		});

		private static readonly Dictionary<string, IncomingPacketConsumer> packets = new()
		{
			{ "ping", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonLegacyPingIncomingMessage) },
			{ "token_login", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonTokenLoginIncomingMessage) },
			{ "mv", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonManageVarsIncomingMessage) },
			{ "set_account_settings", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonSetAccountSettingsMessage) },
			{ "get_level_list", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonGetLevelListIncomingMessage) },
			{ "jr", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonJoinRoomIncomingMessage) },
			{ "lr", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonLeaveRoomIncomingMessage) },
			{ "get_member_list", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonGetMemberListIncomingMessage) },
			{ "get_user_list", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonGetUserListIncomingMessage) },
			{ "get_user_page", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonGetUserPageIncomingMessage) },
			{ "sr", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonSendToRoomIncomingMessage) },
			{ "create_match", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonCreateMatchIncomingMessage) },
			{ "request_matches", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonRequestMatchesIncomingMessage) },
			{ "lose_hat", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonLoseHatIncomingMessage) },
			{ "get_hat", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonGetHatIncomingMessage) },
			{ "coins", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonCoinsIncomingMessage) },
			{ "get_pms", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonGetPmsIncomingMessage) },
			{ "get_pm", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonGetPmIncomingMessage) },
			{ "delete_pms", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonDeletePmsIncomingMessage) },
			{ "send_pm", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonSendPmIncomingMessage) },
			{ "edit_user_list", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonEditUserListIncomingMessage) },
			{ "kick", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonKickFromMatchListingIncomingMessage) },
			{ "ban", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonBanFromMatchListingIncomingMessage) },
			{ "rate_level", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonRateLevelIncomingMessage) },
			{ "send_thing", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonSendThingIncomingMessage) },
			{ "thingExits", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonThingExistsIncomingMessage) },
			{ "accept_thing_transfer", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonAcceptThingTransferIncomingMessage) },
			{ "report_pm", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonReportPmIncomingMessage) },
			{ "koth", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonKothIncomingMessage) },
			{ "dash", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonDashIncomingMessage) },
			{ "win_hat", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonWinHatIncomingMessage) },
			{ "confirm_connection", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonConfirmConnectionIncomingMessage) },
			{ "guest_login", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonGuestLoginIncomingPacket) },
			{ "get_lotd", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonGetLevelOfTheDayIncomingMessage) },
			{ "get_tournament", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonGetTournamentIncomingMessage) },
			{ "gr", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonGetRoomsIncomingMessage) },
			{ "leave_lobby", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonLeaveLobbyIncomingMessage) },
			{ "force_start", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonForceStartIncomingMessage) },
			{ "finish_drawing", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonFinishDrawingIncomingMessage) },
			{ "forfiet", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonForfietIncomingMessage) },
			{ "finish_match", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonFinishMatchIncomingMessage) },
			{ "start_quick_join", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonStartQucikJoinIncomingMessage) },
			{ "stop_quick_join", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonStopQuickJoinIncomingMessage) },
			{ "join_tournament", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketParser.jsonContext.JsonJoinTournamentIncomingMessage) },
		};

		private delegate JsonPacket IncomingPacketConsumer(ref Utf8JsonReader reader);

		public JsonPacket Parse(ref PacketReader reader)
        {
            Utf8JsonReader jsonReader = new(reader.UnreadSequence);

            static string GetPacketType(Utf8JsonReader reader)
            {
	            if (!reader.Read() || reader.TokenType != JsonTokenType.StartObject)
	            {
		            throw new JsonException("Malformed packet");
	            }

	            while (reader.Read())
	            {
		            if (reader.TokenType != JsonTokenType.PropertyName)
		            {
			            reader.Skip();

			            continue;
		            }

		            if ((reader.ValueTextEquals("t") || reader.ValueTextEquals("type")) && reader.Read())
		            {
			            return reader.GetString() ?? throw new JsonException("Packet identifier was null");
		            }
	            }

	            throw new JsonException("Packet is missing the identifier");
            }

            string type = GetPacketType(jsonReader); //Implicit struct copy

            if (!JsonPacketParser.packets.TryGetValue(type, out IncomingPacketConsumer consumer))
            {
	            throw new JsonException($"Unknown packet identifier {type}");
            }

            return consumer(ref jsonReader);
		}
    }
}
