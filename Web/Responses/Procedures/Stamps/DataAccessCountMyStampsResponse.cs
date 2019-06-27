using System.Collections.Generic;
using System.Xml.Serialization;
using static Platform_Racing_3_Web.Responses.Procedures.Stamps.DataAccessCountMyStampsResponse;

namespace Platform_Racing_3_Web.Responses.Procedures.Stamps
{
    public class DataAccessCountMyStampsResponse : DataAccessDataResponse<StampCountResponse>
    {
        private DataAccessCountMyStampsResponse()
        {

        }

        public DataAccessCountMyStampsResponse(string category, uint stampsCount)
        {
            this.Rows = new List<StampCountResponse>()
            {
                new StampCountResponse(category, stampsCount),
            };
        }

        public class StampCountResponse
        {

            [XmlElement("category")]
            public string Category { get; set; }

            [XmlElement("count")]
            public uint StampsCount { get; set; }
            private StampCountResponse()
            {

            }

            public StampCountResponse(string category, uint stampsCount)
            {
                this.Category = category;
                this.StampsCount = stampsCount;
            }
        }
    }
}
