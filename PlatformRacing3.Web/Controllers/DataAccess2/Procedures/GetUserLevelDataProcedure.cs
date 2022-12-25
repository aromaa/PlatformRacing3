using PlatformRacing3.Web.Controllers.DataAccess2.Procedures.Exceptions;
using PlatformRacing3.Web.Extensions;
using PlatformRacing3.Web.Responses;
using PlatformRacing3.Web.Responses.Procedures;
using System.Xml.Linq;
using PlatformRacing3.Common.Level;

namespace PlatformRacing3.Web.Controllers.DataAccess2.Procedures;

public class GetUserLevelDataProcedure : IProcedure
{
	public async Task<IDataAccessDataResponse> GetResponseAsync(HttpContext httpContext, XDocument xml)
	{
		uint userId = httpContext.IsAuthenicatedPr3User();
		if (userId == 0)
		{
			return new DataAccessErrorResponse("You are not logged in!");
		}

		XElement data = xml.Element("Params") ?? throw new DataAccessProcedureMissingData();
		
		uint levelId = (uint?)data.Element("p_level_id") ?? throw new DataAccessProcedureMissingData();

		string levelData = await LevelManager.GetUserLevelData(userId, levelId);
		if (levelData is not null)
		{
			return new DataAccessGetUserLevelDataResponse(levelData);
		}
		else
		{
			return new DataAccessGetUserLevelDataResponse();
		}
	}
}
