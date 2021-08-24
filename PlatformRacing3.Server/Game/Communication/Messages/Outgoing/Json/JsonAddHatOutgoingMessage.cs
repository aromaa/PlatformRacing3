using System.Drawing;
using System.Text.Json.Serialization;
using PlatformRacing3.Common.Customization;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;
using PlatformRacing3.Server.Game.Match;

namespace PlatformRacing3.Server.Game.Communication.Messages.Outgoing.Json
{
    internal sealed class JsonAddHatOutgoingMessage : JsonPacket
    {
        private protected override string InternalType => "addHat";

        [JsonPropertyName("id")]
        public uint Id { get; set; }
        [JsonPropertyName("num")]
        public Hat Hat { get; set; }
        [JsonPropertyName("color")]
        public Color Color { get; set; }

        [JsonPropertyName("x")]
        public double X { get; set; }
        [JsonPropertyName("y")]
        public double Y { get; set; }

        [JsonPropertyName("velX")]
        public float VelX { get; set; }
        [JsonPropertyName("velY")]
        public float VelY { get; set; }

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
