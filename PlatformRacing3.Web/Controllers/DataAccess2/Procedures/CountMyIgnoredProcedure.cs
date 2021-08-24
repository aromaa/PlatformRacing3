using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using PlatformRacing3.Common.User;
using PlatformRacing3.Web.Extensions;
using PlatformRacing3.Web.Responses;
using PlatformRacing3.Web.Responses.Procedures;

namespace PlatformRacing3.Web.Controllers.DataAccess2.Procedures
{
    public class CountMyIgnoredProcedure : IProcedure
    {
        public async Task<IDataAccessDataResponse> GetResponseAsync(HttpContext httpContext, XDocument xml)
        {
            uint userId = httpContext.IsAuthenicatedPr3User();
            if (userId > 0)
            {
                uint count = await UserManager.CountMyIgnoredAsync(userId);

                return new DataAccessCountMyIgnoredProcedureResponse(count);
            }
            else
            {
                return new DataAccessErrorResponse("You are not logged in!");
            }
        }
    }
}
