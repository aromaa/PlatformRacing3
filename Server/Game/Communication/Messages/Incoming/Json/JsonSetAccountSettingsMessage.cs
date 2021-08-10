using System.Text.Json.Serialization;
using Platform_Racing_3_Common.Customization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal sealed class JsonSetAccountSettingsMessage : JsonPacket
    {
        [JsonPropertyName("hat")]
        public Hat Hat { get; set; }
        [JsonPropertyName("hatColor")]
        public Color HatColor { get; set; }

        [JsonPropertyName("head")]
        public Part Head { get; set; }
        [JsonPropertyName("headColor")]
        public Color HeadColor { get; set; }

        [JsonPropertyName("body")]
        public Part Body { get; set; }
        [JsonPropertyName("bodyColor")]
        public Color BodyColor { get; set; }

        [JsonPropertyName("feet")]
        public Part Feet { get; set; }
        [JsonPropertyName("feetColor")]
        public Color FeetColor { get; set; }

        [JsonPropertyName("speed")]
        public uint Speed { get; set; }
        [JsonPropertyName("accel")]
        public uint Accel { get; set; }
        [JsonPropertyName("jump")]
        public uint Jump { get; set; }
    }
}
