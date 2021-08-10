using Platform_Racing_3_Common.Customization;
using Platform_Racing_3_Common.Json;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Platform_Racing_3_Common.Campaign
{
    public class CampaignRun
    {
        [JsonPropertyName("userName")]
        public string Username { get; set; }

        [JsonPropertyName("playbackFreq")]
        public uint PlaybackFreq { get; set; }
        [JsonPropertyName("updateArray")]
        public List<RecordUpdate> Updates { get; set; }

        [JsonPropertyName("hat")]
        public Hat Hat { get; set; }
        [JsonConverter(typeof(JsonColorConverter))]
        [JsonPropertyName("hatColor")]
        public Color HatColor { get; set; }

        [JsonPropertyName("head")]
        public Part Head { get; set; }
        [JsonConverter(typeof(JsonColorConverter))]
        [JsonPropertyName("headColor")]
        public Color HeadColor { get; set; }

        [JsonPropertyName("body")]
        public Part Body { get; set; }
        [JsonConverter(typeof(JsonColorConverter))]
        [JsonPropertyName("bodyColor")]
        public Color BodyColor { get; set; }

        [JsonPropertyName("feet")]
        public Part Feet { get; set; }
        [JsonConverter(typeof(JsonColorConverter))]
        [JsonPropertyName("feetColor")]
        public Color FeetColor { get; set; }
        
        public class RecordUpdate
        {
            /// <summary>
            /// This contains both X and Y seprated by |, ugh
            /// 
            /// This is offset from the last position, the first update contains the start position
            /// </summary>
            [JsonPropertyName("p")]
            public string Position { get; set; }

            [JsonPropertyName("s")]
            public int? ScaleX { get; set; }

            [JsonPropertyName("t")]
            public string State { get; set; }

            [JsonPropertyName("i")]
            public string Item { get; set; }

            [JsonPropertyName("r")]
            public float? Rotation { get; set; }
        }

        public static CampaignRun FromCompressed(Stream stream)
        {
            using (FromBase64Transform base64Transformer = new())
            {
                using (CryptoStream cryptoStream = new(stream, base64Transformer, CryptoStreamMode.Read, leaveOpen: true))
                {
                    //TODO: Read zlib header, .NET 6 has ZlibStream, switch to that and remove this workaround
                    cryptoStream.ReadByte();
                    cryptoStream.ReadByte();

                    using (DeflateStream deflate = new(cryptoStream, CompressionMode.Decompress))
	                {
                        //TODO: .NET 6 has sync method for stream, no need for async->sync
                        return JsonSerializer.DeserializeAsync<CampaignRun>(deflate).GetAwaiter().GetResult();
	                }
                }
            }
        }

        public static CampaignRun FromCompressed(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);

            using (MemoryStream stream = new(bytes))
            {
                return CampaignRun.FromCompressed(stream);
            }
        }
    }
}
