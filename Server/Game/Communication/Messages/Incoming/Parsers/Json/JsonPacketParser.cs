using Net.Communication.Attributes;
using Newtonsoft.Json;
using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Communication.Managers;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using Net.Buffers;
using Net.Communication.Incoming.Parser;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Parsers.Json
{
    [PacketManagerRegister(typeof(BytePacketManager))]
    [PacketParserId(0u)]
    internal class JsonPacketParser : IIncomingPacketParser<JsonPacket>
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new();

        static JsonPacketParser()
        {
            JsonPacketParser.JsonSerializerSettings.Converters.Add(new JsonPacketConverter());
            JsonPacketParser.JsonSerializerSettings.Converters.Add(new JsonColorConverter());
        }

        public JsonPacket Parse(ref PacketReader reader)
        {
            JsonPacket packet = JsonConvert.DeserializeObject<JsonPacket>(reader.ReadFixedString(reader.Remaining), JsonPacketParser.JsonSerializerSettings);

            return packet;
        }
    }
}
