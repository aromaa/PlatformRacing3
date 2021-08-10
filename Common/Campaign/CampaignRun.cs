using Platform_Racing_3_Common.Customization;
using Platform_Racing_3_Common.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.Json.Serialization;

namespace Platform_Racing_3_Common.Campaign
{
    public class CampaignRun
    {
        [JsonPropertyName("userName")]
        public string Username { get; private set; }

        [JsonPropertyName("playbackFreq")]
        public uint PlaybackFreq { get; private set; }
        [JsonPropertyName("updateArray")]
        public List<RecordUpdate> Updates;
        
        [JsonPropertyName("hat")]
        public Hat Hat { get; private set; }
        [JsonConverter(typeof(JsonColorConverter))]
        [JsonPropertyName("hatColor")]
        public Color HatColor { get; private set; }

        [JsonPropertyName("head")]
        public Part Head { get; private set; }
        [JsonConverter(typeof(JsonColorConverter))]
        [JsonPropertyName("headColor")]
        public Color HeadColor { get; private set; }

        [JsonPropertyName("body")]
        public Part Body { get; private set; }
        [JsonConverter(typeof(JsonColorConverter))]
        [JsonPropertyName("bodyColor")]
        public Color BodyColor { get; private set; }

        [JsonPropertyName("feet")]
        public Part Feet { get; private set; }
        [JsonConverter(typeof(JsonColorConverter))]
        [JsonPropertyName("feetColor")]
        public Color FeetColor { get; private set; }
        
        public class RecordUpdate
        {
            /// <summary>
            /// This contains both X and Y seprated by |, ugh
            /// 
            /// This is offset from the last position, the first update contains the start position
            /// </summary>
            [JsonPropertyName("p")]
            public string Position { get; private set; }

            [JsonPropertyName("s")]
            public string ScaleX { get; private set; }

            [JsonPropertyName("t")]
            public string State { get; private set; }

            [JsonPropertyName("i")]
            public string Item { get; private set; }

            [JsonPropertyName("r")]
            public string Rotation { get; private set; }
        }
    }
}
