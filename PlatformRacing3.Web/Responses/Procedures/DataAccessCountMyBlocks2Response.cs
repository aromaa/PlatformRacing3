using System.Collections.Generic;
using System.Xml.Serialization;

namespace PlatformRacing3.Web.Responses.Procedures
{
    public class DataAccessCountMyBlocks2Response : DataAccessDataResponse<DataAccessCountMyBlocks2Response.CategoryData>
    {
        private DataAccessCountMyBlocks2Response()
        {

        }

        public DataAccessCountMyBlocks2Response(string category, uint count)
        {
            this.Rows = new List<CategoryData>()
            {
                new CategoryData(category, count),
            };
        }

        public class CategoryData
        {
            [XmlElement("category")]
            public string Category { get; set; }

            [XmlElement("count")]
            public uint Count { get; set; }

            private CategoryData()
            {

            }

            public CategoryData(string category, uint count)
            {
                this.Category = category;
                this.Count = count;
            }
        }
    }
}
