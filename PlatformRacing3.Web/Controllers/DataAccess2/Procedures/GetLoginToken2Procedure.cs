using System.Xml.Linq;
using PlatformRacing3.Common.Authenication;
using PlatformRacing3.Web.Extensions;
using PlatformRacing3.Web.Responses;
using PlatformRacing3.Web.Responses.Procedures;

namespace PlatformRacing3.Web.Controllers.DataAccess2.Procedures;

public class GetLoginToken2Procedure : IProcedure
{
	public async Task<IDataAccessDataResponse> GetResponseAsync(HttpContext httpContext, XDocument xml)
	{
		uint userId = httpContext.IsAuthenicatedPr3User();
		if (userId > 0)
		{
			return new DataAccessGetLoginToken2Response(await AuthenicationManager.CreateUniqueLoginTokenAsync(userId));
		}
		else
		{
			return new DataAccessErrorResponse("You are not logged in! If you think this is error, please try again. If this error keeps happening, please contact staff.");
		}
	}
}