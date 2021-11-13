using System.Xml.Serialization;

namespace PlatformRacing3.Web.Responses.Procedures
{
	public class DataAccessGetMyBlockCategorysResponse : DataAccessDataResponse<DataAccessGetMyBlockCategorysResponse.BlockCategory>
    {
        public DataAccessGetMyBlockCategorysResponse()
        {
            this.Rows = new List<BlockCategory>(0);
        }

        public DataAccessGetMyBlockCategorysResponse(params string[] categories)
        {
            this.Rows = categories.Select((c) => new BlockCategory(c)).ToList();
        }

        public void AddCategory(string category)
        {
            this.Rows.Add(new BlockCategory(category));
        }

        public class BlockCategory
        {
            [XmlElement("category")]
            public string Category { get; set; }

            private BlockCategory()
            {

            }

            public BlockCategory(string category)
            {
                this.Category = category;
            }
        }
    }
}
