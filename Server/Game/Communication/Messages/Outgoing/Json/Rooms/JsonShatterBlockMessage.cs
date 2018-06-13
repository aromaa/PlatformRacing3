using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json.Rooms
{
    internal class JsonShatterBlockMessage : JsonMessageOutgoingMessage
    {
        internal JsonShatterBlockMessage(string roomName, int tileY, int tileX) : base(roomName, new RoomMessageData("shatterBlock", new ShatterBlockData(tileY, tileX)))
        {
        }

        private class ShatterBlockData
        {
            [JsonProperty("tileY")]
            internal int TileY { get; set; }

            [JsonProperty("tileX")]
            internal int TileX { get; set; }

            internal ShatterBlockData(int tileY, int tileX)
            {
                this.TileY = tileY;
                this.TileX = tileX;
            }
        }
    }
}
