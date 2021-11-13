using System.Xml.Linq;
using PlatformRacing3.Common.User;
using PlatformRacing3.Web.Controllers.DataAccess2.Procedures.Exceptions;
using PlatformRacing3.Web.Responses;
using PlatformRacing3.Web.Responses.Procedures;

namespace PlatformRacing3.Web.Controllers.DataAccess2.Procedures;

public class SearchUsers2Procedure : IProcedure
{
	public async Task<IDataAccessDataResponse> GetResponseAsync(HttpContext httpContext, XDocument xml)
	{
		XElement data = xml.Element("Params");
		if (data != null)
		{
			string name = (string)data.Element("p_name") ?? throw new DataAccessProcedureMissingData();

			IReadOnlyCollection<PlayerUserData> users = await UserManager.SearchUsers(name);

			return new DataAccessSearchUsers2ProcedureResponse(users);
		}
		else
		{
			throw new DataAccessProcedureMissingData();
		}
	}
}