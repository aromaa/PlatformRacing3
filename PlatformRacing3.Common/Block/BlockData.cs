using System.Data.Common;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace PlatformRacing3.Common.Block
{
	public class BlockData : IXmlSerializable
    {
        private const string DELETED_BLOCK_IMAGE_DATA = "v2 | {\"artArray\":[]}";
        private const string DELETED_BLOCK_SETTINGS = "v2 | {\"type\":\"inactive\"}";

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

        private BlockData(uint id)
        {
            this.Id = id;

            this.Title = string.Empty;
            this.Category = string.Empty;
            this.Description = string.Empty;

            this.ImageData = BlockData.DELETED_BLOCK_IMAGE_DATA;
            this.Settings = BlockData.DELETED_BLOCK_SETTINGS;
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

        public static BlockData GetDeletedBlock(uint id) => new(id);
    }
}
