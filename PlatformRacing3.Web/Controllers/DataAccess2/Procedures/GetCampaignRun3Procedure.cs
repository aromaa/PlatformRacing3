using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using PlatformRacing3.Common.Campaign;
using PlatformRacing3.Web.Controllers.DataAccess2.Procedures.Exceptions;
using PlatformRacing3.Web.Responses;
using PlatformRacing3.Web.Responses.Procedures;

namespace PlatformRacing3.Web.Controllers.DataAccess2.Procedures
{
    public class GetCampaignRun3Procedure : IProcedure
    {
        public async Task<IDataAccessDataResponse> GetResponseAsync(HttpContext httpContext, XDocument xml)
        {
            XElement data = xml.Element("Params");
            if (data != null)
            {
                uint levelId = (uint?)data.Element("p_level_id") ?? throw new DataAccessProcedureMissingData();
                uint userId = (uint?)data.Element("p_user_id") ?? throw new DataAccessProcedureMissingData();

                string campaignRun = await CampaignManager.GetRawRunAsync(levelId, userId);
                if (campaignRun != null)
                {
                    return new DataAccessCampaignRun3Response(campaignRun);
                }
                else
                {
                    return new DataAccessCampaignRun3Response();
                }
            }
            else
            {
                throw new DataAccessProcedureMissingData();
            }
        }
    }
}
