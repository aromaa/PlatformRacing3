using System.Collections.Generic;
using System.Xml.Serialization;
using PlatformRacing3.Web.Utils;

namespace PlatformRacing3.Web.Responses.Procedures.Stamps
{
    public class DataAccessSaveStampResponse : DataAccessDataResponse<DataAccessSaveStampResponse.StampSavedResponse>
    {
        public DataAccessSaveStampResponse()
        {
            this.Rows = new List<StampSavedResponse>(1)
            {
                new StampSavedResponse(),
            };
        }

        public DataAccessSaveStampResponse(bool saved)
        {
            this.Rows = new List<StampSavedResponse>(1)
            {
                new StampSavedResponse(saved),
            };
        }

        public class StampSavedResponse
        {
            [XmlElement("saved")]
            public Bit Saved { get; set; }

            public StampSavedResponse()
            {
                this.Saved = false;
            }

            public StampSavedResponse(bool saved)
            {
                this.Saved = saved;
            }
        }
    }
}
