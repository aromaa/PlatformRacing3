using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Platform_Racing_3_Common.Campaign;
using Platform_Racing_3_Web.Extensions;
using Platform_Racing_3_Web.Responses;
using Platform_Racing_3_Web.Responses.Procedures;

namespace Platform_Racing_3_Web.Controllers.DataAccess2.Procedures
{
    public class GetMyFriendsFastestRunsProcedure : IProcedure
    {
        public async Task<IDataAccessDataResponse> GetResponseAsync(HttpContext httpContext, XDocument xml)
        {
            uint userId = httpContext.IsAuthenicatedPr3User();
            if (userId > 0)
            {
                XElement data = xml.Element("Params");
                if (data != null)
                {
                    uint levelId = (uint?)data.Element("p_level_id") ?? throw new DataAccessProcedureMissingData();

                    DataAccessGetMyFriendsFastestRunsResponse response = new DataAccessGetMyFriendsFastestRunsResponse();

                    IReadOnlyDictionary<uint, (int Time, CampaignRun Run)> runs = await CampaignManager.GetFriendRunsAsync(userId, levelId);
                    if (runs != null)
                    {
                        foreach(KeyValuePair<uint, (int Time, CampaignRun Run)> run in runs)
                        {
                            response.AddRun(run.Key, run.Value.Time, run.Value.Run);
                        }
                    }
                    
                    return response;
                }
                else
                {
                    throw new DataAccessProcedureMissingData();
                }
            }
            else
            {
                return new DataAccessErrorResponse("You are not logged in!");
            }
        }
    }
}
