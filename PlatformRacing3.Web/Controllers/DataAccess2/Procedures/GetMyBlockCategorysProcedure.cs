using System.Xml.Linq;
using PlatformRacing3.Common.Block;
using PlatformRacing3.Web.Extensions;
using PlatformRacing3.Web.Responses;
using PlatformRacing3.Web.Responses.Procedures;

namespace PlatformRacing3.Web.Controllers.DataAccess2.Procedures;

public class GetMyBlockCategorysProcedure : IProcedure
{
	public async Task<IDataAccessDataResponse> GetResponseAsync(HttpContext httpContext, XDocument xml)
	{
		uint userId = httpContext.IsAuthenicatedPr3User();
		if (userId > 0)
		{
			DataAccessGetMyBlockCategorysResponse response = new();

			HashSet<string> categories = await BlockManager.GetMyCategorysAsync(userId);
			foreach(string category in categories)
			{
				response.AddCategory(category);
			}

			return response;
		}
		else
		{
			return new DataAccessErrorResponse("You are not logged in!");
		}
	}
}