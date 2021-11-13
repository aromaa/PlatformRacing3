using System.Xml.Linq;
using PlatformRacing3.Common.Campaign;
using PlatformRacing3.Web.Controllers.DataAccess2.Procedures.Exceptions;
using PlatformRacing3.Web.Extensions;
using PlatformRacing3.Web.Responses;
using PlatformRacing3.Web.Responses.Procedures;

namespace PlatformRacing3.Web.Controllers.DataAccess2.Procedures;

public class GetMyFriendsFastestRunsProcedure : IProcedure
{
	public async Task<IDataAccessDataResponse> GetResponseAsync(HttpContext httpContext, XDocument xml)
	{
		uint userId = httpContext.IsAuthenicatedPr3User();
		if (userId > 0)
		{
			XElement data = xml.Element("Params");
			if (data != null)
			{
				uint levelId = (uint?)data.Element("p_level_id") ?? throw new DataAccessProcedureMissingData();

				DataAccessGetMyFriendsFastestRunsResponse response = new();

				IReadOnlyDictionary<uint, (int Time, CampaignRun Run)> runs = await CampaignManager.GetFriendRunsAsync(userId, levelId);
				if (runs != null)
				{
					foreach(KeyValuePair<uint, (int Time, CampaignRun Run)> run in runs)
					{
						response.AddRun(run.Key, run.Value.Time, run.Value.Run);
					}
				}
                    
				return response;
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