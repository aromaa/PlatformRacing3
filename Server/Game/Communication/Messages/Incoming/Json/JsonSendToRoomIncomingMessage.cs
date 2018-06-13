using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal class JsonSendToRoomIncomingMessage : JsonPacket
    {
        [JsonProperty("room_name", Required = Required.Always)]
        internal string RoomName { get; set; }

        [JsonProperty("room_type", Required = Required.Always)]
        internal string RoomType { get; set; }

        [JsonProperty("send_to_self", Required = Required.Always)]
        internal bool SendToSelf { get; set; }

        [JsonProperty("data", Required = Required.Always)]
        internal RoomMessageData Data { get; set; }

        internal class RoomMessageData
        {
            [JsonProperty("type")] //Optional
            internal string Type { get; set; }

            [JsonProperty("data", Required = Required.Always)]
            internal JObject Data { get; set; }
        }
    }
}
