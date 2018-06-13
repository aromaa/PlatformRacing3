using Newtonsoft.Json;
using Platform_Racing_3_Common.Customization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Platform_Racing_3_Server.Game.Match
{
    internal class MatchPlayerHat
    {
        [JsonProperty("id")]
        internal uint Id { get; }

        [JsonProperty("num")]
        internal Hat Hat { get; }

        [JsonProperty("color")]
        internal uint Color { get; }

        internal bool Spawned { get; }

        internal MatchPlayerHat(uint id, Hat hat, Color color, bool spawned = true)
        {
            this.Id = id;
            this.Hat = hat;
            this.Color = (uint)color.ToArgb();
            this.Spawned = spawned;
        }
    }
}
