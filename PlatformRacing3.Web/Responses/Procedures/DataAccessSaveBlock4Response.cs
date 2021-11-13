using System.Xml.Serialization;
using PlatformRacing3.Web.Utils;

namespace PlatformRacing3.Web.Responses.Procedures
{
	public class DataAccessSaveBlock4Response : DataAccessDataResponse<DataAccessSaveBlock4Response.BlockSaveResponse>
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
