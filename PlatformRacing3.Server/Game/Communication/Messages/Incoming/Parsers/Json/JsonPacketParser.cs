using System.Text.Json;
using Net.Buffers;
using Net.Communication.Attributes;
using Net.Communication.Incoming.Parser;
using PlatformRacing3.Server.Game.Communication.Managers;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Parsers.Json;

[PacketManagerRegister(typeof(BytePacketManager))]
[PacketParserId(0u)]
internal sealed class JsonPacketParser : IIncomingPacketParser<JsonPacket>
{
	private static readonly Dictionary<string, IncomingPacketConsumer> packets = new()
	{
		{ "ping", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonLegacyPingIncomingMessage) },
		{ "token_login", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonTokenLoginIncomingMessage) },
		{ "mv", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonManageVarsIncomingMessage) },
		{ "set_account_settings", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonSetAccountSettingsMessage) },
		{ "get_level_list", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonGetLevelListIncomingMessage) },
		{ "jr", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonJoinRoomIncomingMessage) },
		{ "lr", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonLeaveRoomIncomingMessage) },
		{ "get_member_list", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonGetMemberListIncomingMessage) },
		{ "get_user_list", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonGetUserListIncomingMessage) },
		{ "get_user_page", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonGetUserPageIncomingMessage) },
		{ "sr", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonSendToRoomIncomingMessage) },
		{ "create_match", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonCreateMatchIncomingMessage) },
		{ "request_matches", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonRequestMatchesIncomingMessage) },
		{ "lose_hat", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonLoseHatIncomingMessage) },
		{ "get_hat", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonGetHatIncomingMessage) },
		{ "coins", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonCoinsIncomingMessage) },
		{ "get_pms", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonGetPmsIncomingMessage) },
		{ "get_pm", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonGetPmIncomingMessage) },
		{ "delete_pms", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonDeletePmsIncomingMessage) },
		{ "send_pm", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonSendPmIncomingMessage) },
		{ "edit_user_list", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonEditUserListIncomingMessage) },
		{ "kick", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonKickFromMatchListingIncomingMessage) },
		{ "ban", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonBanFromMatchListingIncomingMessage) },
		{ "rate_level", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonRateLevelIncomingMessage) },
		{ "send_thing", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonSendThingIncomingMessage) },
		{ "thingExits", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonThingExistsIncomingMessage) },
		{ "accept_thing_transfer", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonAcceptThingTransferIncomingMessage) },
		{ "report_pm", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonReportPmIncomingMessage) },
		{ "koth", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonKothIncomingMessage) },
		{ "dash", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonDashIncomingMessage) },
		{ "win_hat", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonWinHatIncomingMessage) },
		{ "confirm_connection", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonConfirmConnectionIncomingMessage) },
		{ "guest_login", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonGuestLoginIncomingPacket) },
		{ "get_lotd", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonGetLevelOfTheDayIncomingMessage) },
		{ "get_tournament", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonGetTournamentIncomingMessage) },
		{ "gr", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonGetRoomsIncomingMessage) },
		{ "leave_lobby", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonLeaveLobbyIncomingMessage) },
		{ "force_start", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonForceStartIncomingMessage) },
		{ "finish_drawing", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonFinishDrawingIncomingMessage) },
		{ "forfiet", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonForfietIncomingMessage) },
		{ "finish_match", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonFinishMatchIncomingMessage) },
		{ "start_quick_join", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonStartQucikJoinIncomingMessage) },
		{ "stop_quick_join", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonStopQuickJoinIncomingMessage) },
		{ "join_tournament", static (ref Utf8JsonReader reader) => JsonSerializer.Deserialize(ref reader, JsonPacketContext.Default.JsonJoinTournamentIncomingMessage) },
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