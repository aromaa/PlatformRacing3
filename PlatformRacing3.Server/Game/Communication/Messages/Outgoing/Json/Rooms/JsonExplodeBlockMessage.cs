using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json.Rooms
{
    internal sealed class JsonExplodeBlockMessage : JsonMessageOutgoingMessage
    {
        internal JsonExplodeBlockMessage(string roomName, int tileY, int tileX) : base(roomName, new RoomMessageData("explodeBlock", new ExplodeBlockData(tileY, tileX)))
        {
        }

        private sealed class ExplodeBlockData
        {
            [JsonPropertyName("tileY")]
            public int TileY { get; set; }

            [JsonPropertyName("tileX")]
            public int TileX { get; set; }

            internal ExplodeBlockData(int tileY, int tileX)
            {
                this.TileY = tileY;
                this.TileX = tileX;
            }
        }
    }
}
