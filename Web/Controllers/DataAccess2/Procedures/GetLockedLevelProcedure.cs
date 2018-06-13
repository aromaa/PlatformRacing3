using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Platform_Racing_3_Common.Level;
using Platform_Racing_3_Web.Responses;
using Platform_Racing_3_Web.Responses.Procedures;

namespace Platform_Racing_3_Web.Controllers.DataAccess2.Procedures
{
    public class GetLockedLevelProcedure : IProcedure
    {
        public async Task<IDataAccessDataResponse> GetResponse(HttpContext httpContext, XDocument xml)
        {
            XElement data = xml.Element("Params");
            if (data != null)
            {
                uint levelId = (uint?)data.Element("p_level_id") ?? throw new DataAccessProcedureMissingData();

                LevelData levelData = await LevelManager.GetLevelDataAsync(levelId);
                if (levelData != null && levelData.IsCampaign)
                {
                    return new DataAccessGetLockedLevelResponsee(levelData);
                }
                else
                {
                    return new DataAccessErrorResponse("Level was not found");
                }
            }
            else
            {
                throw new DataAccessProcedureMissingData();
            }
        }
    }
}
