using Platform_Racing_3_Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static Platform_Racing_3_Web.Responses.Procedures.Stamps.DataAccessSaveStampResponse;

namespace Platform_Racing_3_Web.Responses.Procedures.Stamps
{
    public class DataAccessSaveStampResponse : DataAccessDataResponse<StampSavedResponse>
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
