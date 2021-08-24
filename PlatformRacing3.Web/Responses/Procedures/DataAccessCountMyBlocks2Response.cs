using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static Platform_Racing_3_Web.Responses.Procedures.DataAccessCountMyBlocks2Response;

namespace Platform_Racing_3_Web.Responses.Procedures
{
    public class DataAccessCountMyBlocks2Response : DataAccessDataResponse<CategoryData>
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
