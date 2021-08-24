using Platform_Racing_3_Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static Platform_Racing_3_Web.Responses.Procedures.DataAccessSaveBlock4Response;

namespace Platform_Racing_3_Web.Responses.Procedures
{
    public class DataAccessSaveBlock4Response : DataAccessDataResponse<BlockSaveResponse>
    {
        public DataAccessSaveBlock4Response()
        {
            this.Rows = new List<BlockSaveResponse>(1)
            {
                new BlockSaveResponse(),
            };
        }

        public DataAccessSaveBlock4Response(bool saved)
        {
            this.Rows = new List<BlockSaveResponse>(1)
            {
                new BlockSaveResponse(saved),
            };
        }

        public class BlockSaveResponse
        {
            [XmlElement("saved")]
            public Bit Saved { get; set; }

            public BlockSaveResponse()
            {
                this.Saved = false;
            }

            public BlockSaveResponse(bool saved)
            {
                this.Saved = saved;
            }
        }
    }
}
