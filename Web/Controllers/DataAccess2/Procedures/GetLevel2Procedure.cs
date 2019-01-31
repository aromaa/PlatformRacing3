using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Platform_Racing_3_Common.Level;
using Platform_Racing_3_Web.Extensions;
using Platform_Racing_3_Web.Responses;
using Platform_Racing_3_Web.Responses.Procedures;

namespace Platform_Racing_3_Web.Controllers.DataAccess2.Procedures
{
    public class GetLevel2Procedure : IProcedure
    {
        public async Task<IDataAccessDataResponse> GetResponseAsync(HttpContext httpContext, XDocument xml)
        {
            XElement data = xml.Element("Params");
            if (data != null)
            {
                uint levelId = (uint?)data.Element("p_level_id") ?? throw new DataAccessProcedureMissingData();

                LevelData levelData = await LevelManager.GetLevelDataAsync(levelId);
                if (levelData != null)
                {
                    /*uint userId = httpContext.IsAuthenicatedPr3User();
                    if (levelData.Publish || (userId > 0 && levelData.AuthorUserId == userId))
                    {
                        return new DataAccessGetLevel2Response(levelData);
                    }
                    else
                    {
                        return new DataAccessErrorResponse("You may not access unpublished levels!");
                    }*/

                    //TODO: Fix this, only allow accessing unpublished levels if going try matchlisting, can be easily done using redis tokens etc

                    return new DataAccessGetLevel2Response(levelData);
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
