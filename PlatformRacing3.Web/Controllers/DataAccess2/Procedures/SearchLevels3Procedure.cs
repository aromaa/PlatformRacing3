using System.Xml.Linq;
using PlatformRacing3.Common.Level;
using PlatformRacing3.Common.User;
using PlatformRacing3.Web.Controllers.DataAccess2.Procedures.Exceptions;
using PlatformRacing3.Web.Extensions;
using PlatformRacing3.Web.Responses;
using PlatformRacing3.Web.Responses.Procedures;

namespace PlatformRacing3.Web.Controllers.DataAccess2.Procedures;

public class SearchLevels3Procedure : IProcedure
{
	public async Task<IDataAccessDataResponse> GetResponseAsync(HttpContext httpContext, XDocument xml)
	{
		uint userId = httpContext.IsAuthenicatedPr3User();

		XElement data = xml.Element("Params");
		if (data != null)
		{
			string mode = (string)data.Element("p_mode") ?? throw new DataAccessProcedureMissingData();
			string sort = (string)data.Element("p_sort") ?? throw new DataAccessProcedureMissingData();
			string dir = (string)data.Element("p_dir") ?? throw new DataAccessProcedureMissingData();
			string searchStr = (string)data.Element("p_search_str") ?? throw new DataAccessProcedureMissingData();

			IReadOnlyCollection<LevelData> levels = await LevelManager.SearchLevels(mode, sort, dir, searchStr, userId > 0 ? await UserManager.TryGetUserDataByIdAsync(userId) : null);

			return new DataAccessSearchLevels3Response(levels);
		}
		else
		{
			throw new DataAccessProcedureMissingData();
		}
	}
}