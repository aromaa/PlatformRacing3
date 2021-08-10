using Net.Communication.Attributes;
using System.Text.Json.Serialization;
using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Communication.Managers;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json.Converters;
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
        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            Converters =
            {
                new JsonPacketConverter(),
                new JsonColorConverter(),
                new JsonLevelModeConverter(),
                new JsonDateTimeConverter()
            }
        };

        public JsonPacket Parse(ref PacketReader reader)
        {
            Utf8JsonReader jsonReader = new(reader.UnreadSequence);

            JsonPacket packet = JsonSerializer.Deserialize<JsonPacket>(ref jsonReader, JsonPacketParser.jsonSerializerOptions);

            return packet;
        }
    }
}
