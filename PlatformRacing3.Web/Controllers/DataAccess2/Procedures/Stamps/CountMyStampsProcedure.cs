using System.Xml.Linq;
using PlatformRacing3.Common.Stamp;
using PlatformRacing3.Web.Controllers.DataAccess2.Procedures.Exceptions;
using PlatformRacing3.Web.Extensions;
using PlatformRacing3.Web.Responses;
using PlatformRacing3.Web.Responses.Procedures.Stamps;

namespace PlatformRacing3.Web.Controllers.DataAccess2.Procedures.Stamps;

public class CountMyStampsProcedure : IProcedure
{
	public async Task<IDataAccessDataResponse> GetResponseAsync(HttpContext httpContext, XDocument xml)
	{
		uint userId = httpContext.IsAuthenicatedPr3User();
		if (userId > 0)
		{
			XElement data = xml.Element("Params");
			if (data != null)
			{
				string category = (string)data.Element("p_category") ?? throw new DataAccessProcedureMissingData();

				uint count = await StampManager.CountMyStampsAsync(userId, category);

				return new DataAccessCountMyStampsResponse(category, count);
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