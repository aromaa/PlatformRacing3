using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Platform_Racing_3_Common.Campaign;
using Platform_Racing_3_Common.User;
using Platform_Racing_3_Web.Extensions;
using Platform_Racing_3_Web.Responses;
using Platform_Racing_3_Web.Responses.Procedures;

namespace Platform_Racing_3_Web.Controllers.DataAccess2.Procedures
{
    public class SaveCampaignRun3Procedure : IProcedure
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
                    uint levelVersion = (uint?)data.Element("p_level_version") ?? throw new DataAccessProcedureMissingData();
                    string recordRun = (string)data.Element("p_recorded_run") ?? throw new DataAccessProcedureMissingData();
                    int finishTime = (int?)data.Element("p_finish_time") ?? throw new DataAccessProcedureMissingData();

                    CampaignRun campaignRun = CampaignRun.FromCompressed(recordRun);

                    if (campaignRun != null)
                    {
                        PlayerUserData playerUserData = await UserManager.TryGetUserDataByIdAsync(userId);
                        if (campaignRun.Username != playerUserData.Username)
                        {
                            return new DataAccessErrorResponse("Invalid username");
                        }

                        await CampaignManager.SaveCampaignRunAsync(userId, levelId, levelVersion, recordRun, finishTime);

                        return new DataAccessSaveCampaignRun3Response();
                    }
                    else
                    {
                        throw new DataAccessProcedureMissingData();
                    }
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
