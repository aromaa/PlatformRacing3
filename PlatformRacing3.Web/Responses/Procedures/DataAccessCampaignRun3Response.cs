using System.Collections.Generic;
using System.Xml.Serialization;

namespace PlatformRacing3.Web.Responses.Procedures
{
    public class DataAccessCampaignRun3Response : DataAccessDataResponse<DataAccessCampaignRun3Response.CampaignRunResponse>
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
