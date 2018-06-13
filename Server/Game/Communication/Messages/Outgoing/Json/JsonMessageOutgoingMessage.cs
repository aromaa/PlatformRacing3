using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonMessageOutgoingMessage : JsonPacket
    {
        internal override string Type => "receiveMessage";

        [JsonProperty("roomName", DefaultValueHandling = DefaultValueHandling.Ignore)]
        internal string RoomName { get; set; }

        [JsonProperty("socketID", DefaultValueHandling = DefaultValueHandling.Ignore)]
        internal uint SocketId { get; set; }

        [JsonProperty("data")]
        internal RoomMessageData Data { get; set; }

        internal JsonMessageOutgoingMessage(string roomName, RoomMessageData data)
        {
            this.RoomName = roomName;
            this.Data = data;
        }

        internal JsonMessageOutgoingMessage(uint socketId, RoomMessageData data)
        {
            this.SocketId = socketId;
            this.Data = data;
        }

        internal JsonMessageOutgoingMessage(string roomName, uint socketId, RoomMessageData data)
        {
            this.RoomName = roomName;
            this.SocketId = socketId;
            this.Data = data;
        }

        internal class RoomMessageData
        {
            [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
            internal string Type { get; set; }

            [JsonProperty("data")]
            internal object Data { get; set; }

            internal RoomMessageData(string type, object data)
            {
                this.Type = type;
                this.Data = data;
            }

            internal RoomMessageData(object data)
            {
                this.Data = data;
            }
        }
    }
}
