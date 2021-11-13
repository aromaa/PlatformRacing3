using System.Xml.Linq;
using PlatformRacing3.Common.User;
using PlatformRacing3.Web.Controllers.DataAccess2.Procedures.Exceptions;
using PlatformRacing3.Web.Extensions;
using PlatformRacing3.Web.Responses;
using PlatformRacing3.Web.Responses.Procedures;

namespace PlatformRacing3.Web.Controllers.DataAccess2.Procedures
{
	public class GetMyIgnoredProcedure : IProcedure
    {
        public async Task<IDataAccessDataResponse> GetResponseAsync(HttpContext httpContext, XDocument xml)
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
