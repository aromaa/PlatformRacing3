using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using PlatformRacing3.Web.Utils;

namespace PlatformRacing3.Web.Responses.Procedures
{
    public class DataAccessSaveLevel4Response : DataAccessDataResponse<DataAccessSaveLevel4Response.LevelSaveResponse>
    {
        public DataAccessSaveLevel4Response()
        {
            this.Rows = new List<LevelSaveResponse>(1)
            {
                new LevelSaveResponse(),
            };
        }

        public DataAccessSaveLevel4Response(uint levelId)
        {
            this.Rows = new List<LevelSaveResponse>(1)
            {
                new LevelSaveResponse(levelId),
            };
        }

        public class LevelSaveResponse
        {
            [XmlElement("saved")]
            public Bit Saved { get; set; }

            [DefaultValue(0u)]
            [XmlElement("level_id")]
            public uint LevelId { get; set; }

            public LevelSaveResponse()
            {
                this.Saved = false;
            }

            public LevelSaveResponse(uint levelId)
            {
                this.Saved = true;
                this.LevelId = levelId;
            }
        }
    }
}
