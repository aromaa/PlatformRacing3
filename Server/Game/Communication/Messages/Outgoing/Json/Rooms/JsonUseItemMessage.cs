using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json.Rooms
{
    internal class JsonUseItemMessage : JsonMessageOutgoingMessage
    {
        internal JsonUseItemMessage(string roomName, uint socketId, double[] pos) : base(roomName, socketId, new RoomMessageData("useItem", new UseItemData(pos)))
        {
        }

        private class UseItemData
        {
            [JsonProperty("p", Required = Required.Always)]
            internal double[] Pos { get; set; }

            internal UseItemData(double[] pos)
            {
                this.Pos = pos;
            }
        }
    }
}
