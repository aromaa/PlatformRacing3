using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json.Rooms
{
    internal sealed class JsonShatterBlockMessage : JsonMessageOutgoingMessage
    {
        internal JsonShatterBlockMessage(string roomName, int tileY, int tileX) : base(roomName, new RoomMessageData("shatterBlock", new ShatterBlockData(tileY, tileX)))
        {
        }

        private sealed class ShatterBlockData
        {
            [JsonPropertyName("tileY")]
            public int TileY { get; set; }

            [JsonPropertyName("tileX")]
            public int TileX { get; set; }

            internal ShatterBlockData(int tileY, int tileX)
            {
                this.TileY = tileY;
                this.TileX = tileX;
            }
        }
    }
}
