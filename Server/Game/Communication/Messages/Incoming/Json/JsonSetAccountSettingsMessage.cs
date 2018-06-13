using Newtonsoft.Json;
using Platform_Racing_3_Common.Customization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json
{
    internal class JsonSetAccountSettingsMessage : JsonPacket
    {
        [JsonProperty("hat", Required = Required.Always)]
        internal Hat Hat { get; set; }
        [JsonProperty("hatColor", Required = Required.Always)]
        internal Color HatColor { get; set; }

        [JsonProperty("head", Required = Required.Always)]
        internal Part Head { get; set; }
        [JsonProperty("headColor", Required = Required.Always)]
        internal Color HeadColor { get; set; }

        [JsonProperty("body", Required = Required.Always)]
        internal Part Body { get; set; }
        [JsonProperty("bodyColor", Required = Required.Always)]
        internal Color BodyColor { get; set; }

        [JsonProperty("feet", Required = Required.Always)]
        internal Part Feet { get; set; }
        [JsonProperty("feetColor", Required = Required.Always)]
        internal Color FeetColor { get; set; }

        [JsonProperty("speed", Required = Required.Always)]
        internal uint Speed { get; set; }
        [JsonProperty("accel", Required = Required.Always)]
        internal uint Accel { get; set; }
        [JsonProperty("jump", Required = Required.Always)]
        internal uint Jump { get; set; }
    }
}
