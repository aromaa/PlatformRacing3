using Platform_Racing_3_Web.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static Platform_Racing_3_Web.Responses.Procedures.DataAccessSaveLevel4Response;

namespace Platform_Racing_3_Web.Responses.Procedures
{
    public class DataAccessSaveLevel4Response : DataAccessDataResponse<LevelSaveResponse>
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
