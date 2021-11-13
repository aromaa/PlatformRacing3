using System.Data.Common;
using System.Drawing;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using PlatformRacing3.Common.Utils;

namespace PlatformRacing3.Common.Level
{
	public class LevelData : IXmlSerializable
    {
        [JsonPropertyName("levelID")]
        public uint Id { get; }
        [JsonPropertyName("version")]
        public uint Version { get; }

        [XmlElement("user_id")]
        public uint AuthorUserId { get; }
        [JsonPropertyName("author")]
        public string AuthorUsername { get; }
        public Color AuthorNameColor { get; }
        
        [JsonPropertyName("title")]
        public string Title { get; }
        [JsonPropertyName("comment")]
        public string Description { get; }
        [JsonIgnore]
        public bool Publish { get; }

        [JsonIgnore]
        public string SongId { get; }
        [JsonIgnore]
        public LevelMode Mode { get; }

        [JsonIgnore]
        public uint Seconds { get; }
        [JsonIgnore]
        public double Gravity { get; }

        [JsonIgnore]
        public float Alien { get; }
        [JsonIgnore]
        public float Sfchm { get; }
        [JsonIgnore]
        public float Snow { get; }
        [JsonIgnore]
        public float Wind { get; }

        [JsonIgnore]
        public string[] Items { get; }

        [JsonIgnore]
        public uint Health { get; }
        [JsonIgnore]
        public uint[] KingOfTheHat { get; }

        [JsonIgnore]
        public string BgImage { get; }
        [JsonIgnore]
        public string Data { get; }
        [JsonIgnore]
        public string Lua { get; }

        [JsonIgnore]
        public DateTime LastUpdated { get; }
        
        //Below this all fields should be dynamically updated as they are "outside of level data" and should be updated as long as something holds reference to them
        [JsonPropertyName("plays")]
        public uint Plays { get; internal set; }
        [JsonPropertyName("likes")]
        public uint Likes { get; internal set; }
        [JsonPropertyName("dislikes")]
        public uint Dislikes { get; internal set; }

        [JsonIgnore]
        public bool IsCampaign { get; }

        [JsonPropertyName("bronze")]
        public uint BronzeTime { get; internal set; }
        [JsonPropertyName("silver")]
        public uint SilverTime { get; internal set; }
        [JsonPropertyName("gold")]
        public uint GoldTime { get; internal set; }

        [JsonPropertyName("medalsRequired")]
        public uint MedalsRequired { get; internal set; }

        [JsonPropertyName("campaignSeason")]
        public string CampaignSeason { get; internal set; }

        [JsonIgnore]
        public bool HasPrize { get; }

        [JsonIgnore]
        public List<(string prizeType, uint prizeId)> Prizes { get; }

        private LevelData()
        {

        }

        public LevelData(DbDataReader reader)
        {
            this.Id = (uint)(int)reader["id"];
            this.Version = (uint)(int)reader["version"];

            this.AuthorUserId = (uint)(int)reader["author_user_id"];
            this.AuthorUsername = (string)reader["author_username"];
            this.AuthorNameColor = Color.FromArgb((int)reader["author_name_color"]);

            this.Title = (string)reader["title"];
            this.Description = (string)reader["description"];
            this.Publish = (bool)reader["publish"];

            this.SongId = (string)reader["song_id"];
            this.Mode = (LevelMode)reader["mode"];

            this.Seconds = (uint)(int)reader["seconds"];
            this.Gravity = (double)reader["gravity"];

            this.Alien = (float)reader["alien"];
            this.Sfchm = (float)reader["sfchm"];
            this.Snow = (float)reader["snow"];
            this.Wind = (float)reader["wind"];
            
            this.Items = (string[])reader["items"];

            this.Health = (uint)(int)reader["health"];
            this.KingOfTheHat = ((int[])reader["king_of_the_hat"]).Select((i) => (uint)i).ToArray();

            this.BgImage = (string)reader["bg_image"];
            this.Data = (string)reader["level_data"];
            this.Lua = (string)reader["lua"];

            this.LastUpdated = (DateTime)reader["last_updated"];

            this.Plays = (uint)(int)reader["plays"];
            this.Likes = (uint)(long)reader["likes"];
            this.Dislikes = 0; // (uint)(long)reader["dislikes"];

            this.IsCampaign = (bool)reader["is_campaign"];
            if (this.IsCampaign)
            {
                this.BronzeTime = (uint)(int)reader["bronze_time"];
                this.SilverTime = (uint)(int)reader["silver_time"];
                this.GoldTime = (uint)(int)reader["gold_time"];

                this.MedalsRequired = (uint)(int)reader["medals_required"];
                this.CampaignSeason = (string)reader["campaign_season"];
            }

            this.HasPrize = (bool)reader["has_prize"];
            if (this.HasPrize)
            {
                this.Prizes = new List<(string, uint)>();

                object[][] prizes = (object[][])reader["prizes"];
                for (int i = 0; i < prizes.Length; i++)
                {
                    string prizeType = ((string)prizes[i][0]).ToLower();
                    uint prizeId = Convert.ToUInt32(prizes[i][1]);

                    this.Prizes.Add((prizeType, prizeId));
                }
            }
        }

        [JsonPropertyName("author_name_color")]
        public uint AuthorNameColorArgb => (uint)this.AuthorNameColor.ToArgb();

        [JsonPropertyName("mode")]
        public string StringMode
        {
            get
            {
                string mode = this.Mode.ToString();

                return Char.ToLowerInvariant(mode[0]) + mode[1..];
            }
        }

        public IReadOnlyDictionary<string, object> GetVars(params string[] vars) => JsonUtils.GetVars(this, vars);
        public IReadOnlyDictionary<string, object> GetVars(HashSet<string> vars) => JsonUtils.GetVars(this, vars);

        public XmlSchema GetSchema() => null;

        public void ReadXml(XmlReader reader) => throw new NotSupportedException();
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("level_id", this.Id.ToString());
            writer.WriteElementString("version", this.Version.ToString());

            writer.WriteElementString("user_id", this.AuthorUserId.ToString());
            writer.WriteElementString("author_name_color", this.AuthorNameColor.ToArgb().ToString());
            writer.WriteElementString("author_username", this.AuthorUsername);

            writer.WriteElementString("title", this.Title.ToString());
            writer.WriteElementString("comment", this.Description.ToString());
            writer.WriteElementString("publish", this.Publish ? "1" : "0");
            
            writer.WriteElementString("song_id", this.SongId.ToString());
            writer.WriteElementString("mode", this.StringMode);

            writer.WriteElementString("seconds", this.Seconds.ToString());
            writer.WriteElementString("gravity", this.Gravity.ToString(CultureInfo.InvariantCulture));

            writer.WriteElementString("alienChance", this.Alien.ToString());
            writer.WriteElementString("sfchm_chance", this.Sfchm.ToString());
            writer.WriteElementString("wind_chance", this.Wind.ToString());
            writer.WriteElementString("snow_chance", this.Snow.ToString());

            writer.WriteElementString("items", string.Join(',', this.Items));
            writer.WriteElementString("health", this.Health.ToString());
            writer.WriteElementString("king_of_the_hat", string.Join(':', this.KingOfTheHat));

            writer.WriteElementString("bg_image", this.BgImage.ToString());
            writer.WriteElementString("level_data", this.Data);
            writer.WriteElementString("lua", this.Lua);

            writer.WriteElementString("last_updated", this.LastUpdated.ToString());

            writer.WriteElementString("plays", this.Plays.ToString());
            writer.WriteElementString("likes", this.Likes.ToString());
            writer.WriteElementString("dislikes", this.Dislikes.ToString());

            writer.WriteElementString("bronze", this.BronzeTime.ToString());
            writer.WriteElementString("silver", this.SilverTime.ToString());
            writer.WriteElementString("gold", this.GoldTime.ToString());

            writer.WriteElementString("medals_required", this.MedalsRequired.ToString());
            writer.WriteElementString("campaign_sesion", this.CampaignSeason);
        }
    }
}
