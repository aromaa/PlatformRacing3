using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonStartGameMessage : JsonMessageOutgoingMessage
    {
        internal JsonStartGameMessage(string roomName, string gameName) : base(roomName, new StartGameData(gameName))
        {
        }

        private class StartGameData : RoomMessageData
        {
            [JsonProperty("gameName", Required = Required.Always)]
            internal string GameName { get; set; }

            internal StartGameData(string gameName) : base("startGame", null)
            {
                this.GameName = gameName;
            }
        }
    }
}
