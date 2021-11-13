using System.Xml.Linq;
using PlatformRacing3.Common.Server;
using PlatformRacing3.Web.Responses;
using PlatformRacing3.Web.Responses.Procedures;

namespace PlatformRacing3.Web.Controllers.DataAccess2.Procedures;

public class GetServers2Procedure : IProcedure
{
	private readonly ServerManager serverManager;

	public GetServers2Procedure(ServerManager serverManager)
	{
		this.serverManager = serverManager;
	}

	public Task<IDataAccessDataResponse> GetResponseAsync(HttpContext httpContext, XDocument xml)
	{
		return Task.FromResult<IDataAccessDataResponse>(new DataAccessGetServers2Response(this.serverManager.GetServers()));
	}
}