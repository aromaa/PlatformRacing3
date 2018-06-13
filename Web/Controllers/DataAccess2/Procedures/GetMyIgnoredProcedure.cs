using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Platform_Racing_3_Common.User;
using Platform_Racing_3_Web.Extensions;
using Platform_Racing_3_Web.Responses;
using Platform_Racing_3_Web.Responses.Procedures;

namespace Platform_Racing_3_Web.Controllers.DataAccess2.Procedures
{
    public class GetMyIgnoredProcedure : IProcedure
    {
        public async Task<IDataAccessDataResponse> GetResponse(HttpContext httpContext, XDocument xml)
        {
            uint userId = httpContext.IsAuthenicatedPr3User();
            if (userId > 0)
            {
                XElement data = xml.Element("Params");
                if (data != null)
                {
                    uint start = (uint?)data.Element("p_start") ?? throw new DataAccessProcedureMissingData();
                    uint count = (uint?)data.Element("p_count") ?? throw new DataAccessProcedureMissingData();

                    IReadOnlyCollection<PlayerUserData> ignored = await UserManager.GetMyIgnoredAsync(userId, start, count);

                    return new DataAccessGetMyIgnoredProcedureResponse(ignored);
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
