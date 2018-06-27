using Newtonsoft.Json;
using Platform_Racing_3_Common.Customization;
using Platform_Racing_3_Common.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Platform_Racing_3_Common.Campaign
{
    public class CampaignRun
    {
        [JsonProperty("userName", Required = Required.Always)]
        public string Username { get; private set; }

        [JsonProperty("playbackFreq", Required = Required.Always)]
        public uint PlaybackFreq { get; private set; }
        [JsonProperty("updateArray", Required = Required.Always)]
        public List<RecordUpdate> Updates;
        
        [JsonProperty("hat", Required = Required.Always)]
        public Hat Hat { get; private set; }
        [JsonConverter(typeof(ColorJsonConverter))]
        [JsonProperty("hatColor", Required = Required.Always)]
        public Color HatColor { get; private set; }

        [JsonProperty("head", Required = Required.Always)]
        public Part Head { get; private set; }
        [JsonConverter(typeof(ColorJsonConverter))]
        [JsonProperty("headColor", Required = Required.Always)]
        public Color HeadColor { get; private set; }

        [JsonProperty("body", Required = Required.Always)]
        public Part Body { get; private set; }
        [JsonConverter(typeof(ColorJsonConverter))]
        [JsonProperty("bodyColor", Required = Required.Always)]
        public Color BodyColor { get; private set; }

        [JsonProperty("feet", Required = Required.Always)]
        public Part Feet { get; private set; }
        [JsonConverter(typeof(ColorJsonConverter))]
        [JsonProperty("feetColor", Required = Required.Always)]
        public Color FeetColor { get; private set; }
        
        public class RecordUpdate
        {
            /// <summary>
            /// This contains both X and Y seprated by |, ugh
            /// 
            /// This is offset from the last position, the first update contains the start position
            /// </summary>
            [JsonProperty("p", Required = Required.Always)]
            public string Position { get; private set; }

            [JsonProperty("s")]
            public string ScaleX { get; private set; }

            [JsonProperty("t")]
            public string State { get; private set; }

            [JsonProperty("i")]
            public string Item { get; private set; }

            [JsonProperty("r")]
            public string Rotation { get; private set; }
        }
    }
}
