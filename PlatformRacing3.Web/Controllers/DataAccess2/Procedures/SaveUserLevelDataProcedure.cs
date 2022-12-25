using PlatformRacing3.Web.Controllers.DataAccess2.Procedures.Exceptions;
using PlatformRacing3.Web.Extensions;
using PlatformRacing3.Web.Responses;
using PlatformRacing3.Web.Responses.Procedures;
using System.Xml.Linq;
using PlatformRacing3.Common.Level;

namespace PlatformRacing3.Web.Controllers.DataAccess2.Procedures;

public class SaveUserLevelDataProcedure : IProcedure
{
	public async Task<IDataAccessDataResponse> GetResponseAsync(HttpContext httpContext, XDocument xml)
	{
		uint userId = httpContext.IsAuthenicatedPr3User();
		if (userId == 0)
		{
			return new DataAccessErrorResponse("You are not logged in!");
		}

		XElement data = xml.Element("Params");
		if (data is null)
		{
			throw new DataAccessProcedureMissingData();
		}

		uint levelId = (uint?)data.Element("p_level_id") ?? throw new DataAccessProcedureMissingData();
		string levelData = (string)data.Element("p_level_data") ?? throw new DataAccessProcedureMissingData();

		if (levelData.Length >= 1024 * 64)
		{
			return new DataAccessErrorResponse("User level data is limited to the maximum size of 64KB");
		}

		await LevelManager.SaveUserLevelData(userId, levelId, levelData);

		return new DataAccessSaveUserLevelDataResponse();
	}
}
