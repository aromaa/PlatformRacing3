using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json.Rooms
{
    internal class JsonExplodeBlockMessage : JsonMessageOutgoingMessage
    {
        internal JsonExplodeBlockMessage(string roomName, int tileY, int tileX) : base(roomName, new RoomMessageData("explodeBlock", new ExplodeBlockData(tileY, tileX)))
        {
        }

        private class ExplodeBlockData
        {
            [JsonProperty("tileY")]
            internal int TileY { get; set; }

            [JsonProperty("tileX")]
            internal int TileX { get; set; }

            internal ExplodeBlockData(int tileY, int tileX)
            {
                this.TileY = tileY;
                this.TileX = tileX;
            }
        }
    }
}
