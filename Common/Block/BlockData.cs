using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Platform_Racing_3_Common.Block
{
    public class BlockData : IXmlSerializable
    {
        public uint Id { get; }
        public uint Version { get; }

        public uint AuthorUserId { get; }
        
        public string Title { get; }
        public string Category { get; }
        public string Description { get; }

        public string ImageData { get; }
        public string Settings { get; }

        public DateTime LastUpdated { get; }

        private BlockData()
        {

        }

        public BlockData(DbDataReader reader)
        {
            this.Id = (uint)(int)reader["id"];
            this.Version = (uint)(int)reader["version"];

            this.AuthorUserId = (uint)(int)reader["author_user_id"];

            this.Title = (string)reader["title"];
            this.Category = (string)reader["category"];
            this.Description = (string)reader["description"];

            this.ImageData = (string)reader["image_data"];
            this.Settings = (string)reader["settings"];

            this.LastUpdated = (DateTime)reader["last_updated"];
        }

        public XmlSchema GetSchema() => null;

        public void ReadXml(XmlReader reader) => throw new NotSupportedException();
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("block_id", this.Id.ToString());

            writer.WriteElementString("title", this.Title);
            writer.WriteElementString("category", this.Category);
            writer.WriteElementString("comment", this.Description);

            writer.WriteElementString("image_data", this.ImageData);
            writer.WriteElementString("settings", this.Settings);
        }
    }
}
