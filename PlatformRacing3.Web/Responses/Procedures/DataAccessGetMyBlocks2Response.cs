using System.Xml.Serialization;

namespace PlatformRacing3.Web.Responses.Procedures
{
	public class DataAccessGetMyBlocks2Response : DataAccessDataResponse<DataAccessGetMyBlocks2Response.MyBlocksData>
    {
        [XmlElement("category")]
        public string Category { get; set; }

        private DataAccessGetMyBlocks2Response()
        {

        }

        public DataAccessGetMyBlocks2Response(string category)
        {
            this.Rows = new List<MyBlocksData>();

            this.Category = category;
        }

        public void AddBlock(uint blockId)
        {
            this.Rows.Add(new MyBlocksData(blockId));
        }

        public class MyBlocksData
        {
            [XmlElement("block_id")]
            public uint BlockId { get; set; }

            private MyBlocksData()
            {

            }

            public MyBlocksData(uint blockId)
            {
                this.BlockId = blockId;
            }
        }
    }
}
