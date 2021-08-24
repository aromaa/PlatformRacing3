using System;
using System.Data.Common;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace PlatformRacing3.Common.Stamp
{
    public class StampData : IXmlSerializable
    {
        private const string DELETED_ART_DATA = "v2 | {\"artArray\":[]}";

        public uint Id { get; }
        public uint Version { get; }

        public uint AuthorUserId { get; }

        public string Title { get; }
        public string Category { get; }
        public string Description { get; }

        public string Art { get; }

        public DateTime LastUpdated { get; }

        private StampData()
        {

        }

        private StampData(uint id)
        {
            this.Id = id;

            this.Title = string.Empty;
            this.Category = string.Empty;
            this.Description = string.Empty;

            this.Art = StampData.DELETED_ART_DATA;
        }

        public StampData(DbDataReader reader)
        {
            this.Id = (uint)(int)reader["id"];
            this.Version = (uint)(int)reader["version"];

            this.AuthorUserId = (uint)(int)reader["author_user_id"];

            this.Title = (string)reader["title"];
            this.Category = (string)reader["category"];
            this.Description = (string)reader["description"];

            this.Art = (string)reader["art"];

            this.LastUpdated = (DateTime)reader["last_updated"];
        }

        public XmlSchema GetSchema() => null;

        public void ReadXml(XmlReader reader) => throw new NotSupportedException();
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("stamp_id", this.Id.ToString());

            writer.WriteElementString("title", this.Title);
            writer.WriteElementString("category", this.Category);
            writer.WriteElementString("comment", this.Description);

            writer.WriteElementString("art", this.Art);
        }

        public static StampData GetDeletedStamp(uint id) => new(id);
    }
}
