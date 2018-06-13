using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Platform_Racing_3_Common.Database;
using Platform_Racing_3_Common.Level;
using Platform_Racing_3_Web.Extensions;
using Platform_Racing_3_Web.Responses;
using Platform_Racing_3_Web.Responses.Procedures;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Platform_Racing_3_Web.Controllers.DataAccess2.Procedures
{
    public class CountMyLevels2Procedure : IProcedure
    {
        public async Task<IDataAccessDataResponse> GetResponse(HttpContext httpContext, XDocument xml)
        {
            uint userId = httpContext.IsAuthenicatedPr3User();
            if (userId > 0)
            {
                uint count = await LevelManager.CountMyLevelsAsync(userId);

                return new DataAccessCountMyLevels2Response(count);
            }
            else
            {
                return new DataAccessErrorResponse("You are not logged in!");
            }
        }
    }
}
