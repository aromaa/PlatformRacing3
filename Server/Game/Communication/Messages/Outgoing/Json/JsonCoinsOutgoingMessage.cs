using Newtonsoft.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Match;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonCoinsOutgoingMessage : JsonPacket
    {
        internal override string Type => "coins";

        [JsonProperty("array")]
        internal IReadOnlyCollection<PlayerCoinsData> Array { get; set; }

        internal JsonCoinsOutgoingMessage(IReadOnlyCollection<MatchPlayer> matchPlayer)
        {
            this.Array = matchPlayer.Select((p) => new PlayerCoinsData(p)).ToList();
        }

        internal class PlayerCoinsData
        {
            [JsonProperty("socketID")]
            internal uint SocketId { get; set; }

            [JsonProperty("name")]
            internal string Name { get; set; }

            [JsonProperty("finish_time", DefaultValueHandling = DefaultValueHandling.Ignore)]
            internal string FinishTime { get; set; }

            [JsonProperty("finish_place", DefaultValueHandling = DefaultValueHandling.Ignore)]
            internal int? FinishPlace { get; set; }

            [JsonProperty("coins", DefaultValueHandling = DefaultValueHandling.Ignore)]
            internal uint Coins { get; set; }

            [JsonProperty("koth", DefaultValueHandling = DefaultValueHandling.Ignore)]
            internal string Koth { get; set; }

            [JsonProperty("dash", DefaultValueHandling = DefaultValueHandling.Ignore)]
            internal uint Dash { get; set; }

            [JsonProperty("gone")]
            internal bool Gone { get; set; }

            internal PlayerCoinsData(MatchPlayer matchPlayer)
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
