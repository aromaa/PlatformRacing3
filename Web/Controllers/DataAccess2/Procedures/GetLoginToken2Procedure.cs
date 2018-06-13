using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Platform_Racing_3_Common.Authenication;
using Platform_Racing_3_Web.Extensions;
using Platform_Racing_3_Web.Responses;
using Platform_Racing_3_Web.Responses.Procedures;

namespace Platform_Racing_3_Web.Controllers.DataAccess2.Procedures
{
    public class GetLoginToken2Procedure : IProcedure
    {
        public Task<IDataAccessDataResponse> GetResponse(HttpContext httpContext, XDocument xml)
        {
            uint userId = httpContext.IsAuthenicatedPr3User();
            if (userId > 0)
            {
                return Task.FromResult<IDataAccessDataResponse>(new DataAccessGetLoginToken2Response(AuthenicationManager.CreateUniqueLoginToken(userId)));
            }
            else
            {
                return Task.FromResult<IDataAccessDataResponse>(new DataAccessErrorResponse("You are not logged in! If you think this is error, please try again. If this error keeps happening, please contact staff."));
            }
        }
    }
}
