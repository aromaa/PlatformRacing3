using Platform_Racing_3_Common.Stamp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static Platform_Racing_3_Web.Responses.Procedures.Stamps.DataAccessGetMyStampCategories;

namespace Platform_Racing_3_Web.Responses.Procedures.Stamps
{
    public class DataAccessGetMyStampCategories : DataAccessDataResponse<StampCategory>
    {
        private DataAccessGetMyStampCategories()
        {
            this.Rows = new List<StampCategory>();
        }

        public DataAccessGetMyStampCategories(params string[] categories)
        {
            this.Rows = categories.Select((c) => new StampCategory(c)).ToList();
        }

        public void AddCategory(string category)
        {
            this.Rows.Add(new StampCategory(category));
        }

        public class StampCategory
        {
            [XmlElement("category")]
            public string Category { get; set; }

            private StampCategory()
            {

            }

            public StampCategory(string category)
            {
                this.Category = category;
            }
        }
    }
}
