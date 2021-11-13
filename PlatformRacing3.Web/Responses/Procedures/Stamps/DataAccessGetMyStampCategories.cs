using System.Xml.Serialization;

namespace PlatformRacing3.Web.Responses.Procedures.Stamps
{
	public class DataAccessGetMyStampCategories : DataAccessDataResponse<DataAccessGetMyStampCategories.StampCategory>
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
