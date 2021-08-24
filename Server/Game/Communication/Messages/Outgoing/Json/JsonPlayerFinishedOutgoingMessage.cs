using System.Text.Json.Serialization;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Match;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonPlayerFinishedOutgoingMessage : JsonPacket
    {
        private protected override string InternalType => "playerFinished";

        [JsonPropertyName("socketID")]
        public uint SocketId { get; set; }

        [JsonPropertyName("finishArray")]
        public IReadOnlyCollection<PlayerFinishedData> FinishArray { get; set; }

        internal JsonPlayerFinishedOutgoingMessage(uint socketId, IReadOnlyCollection<MatchPlayer> matchPlayer)
        {
            this.SocketId = socketId;
            this.FinishArray = matchPlayer.Select((p) => new PlayerFinishedData(p)).ToList();
        }

        internal sealed class PlayerFinishedData
        {
            [JsonPropertyName("socketID")]
            public uint SocketId { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("finish_time")]
            public string FinishTime { get; set; }

            [JsonPropertyName("finish_place")]
            public int? FinishPlace { get; set; }

            [JsonPropertyName("coins")]
            public uint Coins { get; set; }

            [JsonPropertyName("koth")]
            public string Koth { get; set; }

            [JsonPropertyName("dash")]
            public uint Dash { get; set; }

            [JsonPropertyName("gone")]
            public bool Gone { get; set; }

            internal PlayerFinishedData(MatchPlayer matchPlayer)
            {
                this.SocketId = matchPlayer.SocketId;
                this.Name = matchPlayer.UserData.Username;
                this.FinishTime = matchPlayer.Forfiet ? "forfeit" : matchPlayer.FinishTime?.ToString(CultureInfo.InvariantCulture) ?? string.Empty;
                this.FinishPlace = matchPlayer.FinishPlace;
                this.Koth = matchPlayer.Koth;
                this.Coins = matchPlayer.Coins;
                this.Dash = matchPlayer.Dash;
                this.Gone = matchPlayer.Gone;
            }
        }
    }
}
