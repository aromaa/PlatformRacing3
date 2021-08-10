using System.Text.Json.Serialization;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonStartGameMessage : JsonMessageOutgoingMessage
    {
        internal JsonStartGameMessage(string roomName, string gameName) : base(roomName, new StartGameData(gameName))
        {
        }

        internal sealed class StartGameData : RoomMessageData
        {
            [JsonPropertyName("gameName")]
            public string GameName { get; set; }

            internal StartGameData(string gameName) : base("startGame", null)
            {
                this.GameName = gameName;
            }
        }
    }
}
