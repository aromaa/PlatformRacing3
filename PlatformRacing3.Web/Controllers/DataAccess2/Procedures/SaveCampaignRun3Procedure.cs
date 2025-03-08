using PlatformRacing3.Common.Campaign;
using PlatformRacing3.Common.User;
using PlatformRacing3.Web.Controllers.DataAccess2.Procedures.Exceptions;
using PlatformRacing3.Web.Extensions;
using PlatformRacing3.Web.Responses;
using PlatformRacing3.Web.Responses.Procedures;
using System.Xml.Linq;

namespace PlatformRacing3.Web.Controllers.DataAccess2.Procedures;

public class SaveCampaignRun3Procedure : IProcedure
{
	public async Task<IDataAccessDataResponse> GetResponseAsync(HttpContext httpContext, XDocument xml)
	{
		uint userId = httpContext.IsAuthenicatedPr3User();
		if (userId > 0)
		{
			XElement data = xml.Element("Params");
			if (data != null)
			{
				string category = (string)data.Element("p_category") ?? "normal";
				uint levelId = (uint?)data.Element("p_level_id") ?? throw new DataAccessProcedureMissingData();
				uint levelVersion = (uint?)data.Element("p_level_version") ?? throw new DataAccessProcedureMissingData();
				string recordRun = (string)data.Element("p_recorded_run") ?? throw new DataAccessProcedureMissingData();
				int finishTime = (int?)data.Element("p_finish_time") ?? throw new DataAccessProcedureMissingData();

				CampaignRun campaignRun = CampaignRun.FromCompressed(recordRun);

				if (campaignRun != null)
				{
					PlayerUserData playerUserData = await UserManager.TryGetUserDataByIdAsync(userId);
					if (campaignRun.Username != playerUserData.Username)
					{
						return new DataAccessErrorResponse("Invalid username");
					}

					await CampaignManager.SaveCampaignRunAsync(userId, category, levelId, levelVersion, recordRun, finishTime);

					return new DataAccessSaveCampaignRun3Response();
				}
				else
				{
					throw new DataAccessProcedureMissingData();
				}
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