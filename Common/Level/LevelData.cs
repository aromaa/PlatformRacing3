using Newtonsoft.Json;
using Platform_Racing_3_Common.Exceptions;
using Platform_Racing_3_Common.User;
using Platform_Racing_3_Common.Utils;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Platform_Racing_3_Common.Level
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LevelData : IXmlSerializable
    {
        [JsonProperty("levelID")]
        public uint Id { get; }
        [JsonProperty("version")]
        public uint Version { get; }

        [XmlElement("user_id")]
        public uint AuthorUserId { get; }
        [JsonProperty("author")]
        public string AuthorUsername { get; }
        public Color AuthorNameColor { get; }
        
        [JsonProperty("title")]
        public string Title { get; }
        [JsonProperty("comment")]
        public string Description { get; }
        public bool Publish { get; }

        public string SongId { get; }
        public LevelMode Mode { get; }

        public uint Seconds { get; }
        public double Gravity { get; }

        public float Alien { get; }
        public float Sfchm { get; }
        public float Snow { get; }
        public float Wind { get; }

        public string[] Items { get; }

        public uint Health { get; }
        public uint[] KingOfTheHat { get; }

        public string BgImage { get; }
        public string Data { get; }

        public DateTime LastUpdated { get; }
        
        //Below this all fields should be dynamically updated as they are "outside of level data" and should be updated as long as something holds reference to them
        [JsonProperty("plays")]
        public uint Plays { get; internal set; }
        [JsonProperty("likes")]
        public uint Likes { get; internal set; }
        [JsonProperty("dislikes")]
        public uint Dislikes { get; internal set; }

        public bool IsCampaign { get; }

        [JsonProperty("bronze")]
        public uint BronzeTime { get; internal set; }
        [JsonProperty("silver")]
        public uint SilverTime { get; internal set; }
        [JsonProperty("gold")]
        public uint GoldTime { get; internal set; }

        [JsonProperty("medalsRequired")]
        public uint MedalsRequired { get; internal set; }

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

            this.LastUpdated = (DateTime)reader["last_updated"];

            this.Plays = (uint)(int)reader["plays"];
            this.Likes = (uint)(long)reader["likes"];
            this.Dislikes = (uint)(long)reader["dislikes"];

            this.IsCampaign = (bool)reader["is_campaign"];
            if (this.IsCampaign)
            {
                this.BronzeTime = (uint)(int)reader["bronze_time"];
                this.SilverTime = (uint)(int)reader["silver_time"];
                this.GoldTime = (uint)(int)reader["gold_time"];

                this.MedalsRequired = (uint)(int)reader["medals_required"];
            }
        }

        [JsonProperty("author_name_color")]
        public uint AuthorNameColorArgb => (uint)this.AuthorNameColor.ToArgb();

        [JsonProperty("mode")]
        public string StringMode
        {
            get
            {
                string mode = this.Mode.ToString();

                return Char.ToLowerInvariant(mode[0]) + mode.Substring(1);
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
            writer.WriteElementString("gravity", this.Gravity.ToString());

            writer.WriteElementString("alienChance", this.Alien.ToString());
            writer.WriteElementString("sfchm_chance", this.Sfchm.ToString());
            writer.WriteElementString("wind_chance", this.Wind.ToString());
            writer.WriteElementString("snow_chance", this.Snow.ToString());

            writer.WriteElementString("items", string.Join(',', this.Items));
            writer.WriteElementString("health", this.Health.ToString());
            writer.WriteElementString("king_of_the_hat", string.Join(':', this.KingOfTheHat));

            writer.WriteElementString("bg_image", this.BgImage.ToString());
            writer.WriteElementString("level_data", this.Data);

            writer.WriteElementString("last_updated", this.LastUpdated.ToString());

            writer.WriteElementString("plays", this.Plays.ToString());
            writer.WriteElementString("likes", this.Likes.ToString());
            writer.WriteElementString("dislikes", this.Dislikes.ToString());

            writer.WriteElementString("bronze", this.BronzeTime.ToString());
            writer.WriteElementString("silver", this.SilverTime.ToString());
            writer.WriteElementString("gold", this.GoldTime.ToString());

            writer.WriteElementString("medals_required", this.MedalsRequired.ToString());
        }
    }
}
