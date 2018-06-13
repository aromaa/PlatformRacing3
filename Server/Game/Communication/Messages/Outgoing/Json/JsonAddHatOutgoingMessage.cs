using Newtonsoft.Json;
using Platform_Racing_3_Common.Customization;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Match;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json
{
    internal class JsonAddHatOutgoingMessage : JsonPacket
    {
        internal override string Type => "addHat";

        [JsonProperty("id")]
        internal uint Id { get; set; }
        [JsonProperty("num")]
        internal Hat Hat { get; set; }
        [JsonProperty("color")]
        internal uint Color { get; set; }

        [JsonProperty("x")]
        internal double X { get; set; }
        [JsonProperty("y")]
        internal double Y { get; set; }

        [JsonProperty("velX")]
        internal float VelX { get; set; }
        [JsonProperty("velY")]
        internal float VelY { get; set; }

        internal JsonAddHatOutgoingMessage(MatchPlayerHat hat, double x, double y, float velX, float velY)
        {
            this.Id = hat.Id;
            this.Hat = hat.Hat;
            this.Color = hat.Color;

            this.X = x;
            this.Y = y;

            this.VelX = velX;
            this.VelY = velY;
        }
    }
}
