using System.Xml.Linq;
using PlatformRacing3.Common.Stamp;
using PlatformRacing3.Web.Extensions;
using PlatformRacing3.Web.Responses;
using PlatformRacing3.Web.Responses.Procedures.Stamps;

namespace PlatformRacing3.Web.Controllers.DataAccess2.Procedures.Stamps
{
	public class GetMyStampCategoriesProcedure : IProcedure
    {
        public async Task<IDataAccessDataResponse> GetResponseAsync(HttpContext httpContext, XDocument xml)
        {
            uint userId = httpContext.IsAuthenicatedPr3User();
            if (userId > 0)
            {
                DataAccessGetMyStampCategories categories = new();

                foreach(string category in await StampManager.GetMyStampCategoriesAsync(userId))
                {
                    categories.AddCategory(category);
                }

                return categories;
            }
            else
            {
                return new DataAccessErrorResponse("You are not logged in!");
            }
        }
    }
}
