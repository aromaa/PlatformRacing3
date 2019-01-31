using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static Platform_Racing_3_Web.Responses.Procedures.DataAccessCampaignRun3Response;

namespace Platform_Racing_3_Web.Responses.Procedures
{
    public class DataAccessCampaignRun3Response : DataAccessDataResponse<CampaignRunResponse>
    {
        public DataAccessCampaignRun3Response()
        {
        }

        public DataAccessCampaignRun3Response(string run)
        {
            this.Rows = new List<CampaignRunResponse>(1)
            {
                new CampaignRunResponse(run),
            };
        }

        public class CampaignRunResponse
        {
            [XmlElement("recorded_run")]
            public string Run { get; set; }

            public CampaignRunResponse()
            {

            }

            public CampaignRunResponse(string run)
            {
                this.Run = run;
            }
        }
    }
}
