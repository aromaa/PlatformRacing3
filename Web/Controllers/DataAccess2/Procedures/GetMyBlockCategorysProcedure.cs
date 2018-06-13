using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Platform_Racing_3_Common.Block;
using Platform_Racing_3_Common.Database;
using Platform_Racing_3_Web.Extensions;
using Platform_Racing_3_Web.Responses;
using Platform_Racing_3_Web.Responses.Procedures;

namespace Platform_Racing_3_Web.Controllers.DataAccess2.Procedures
{
    public class GetMyBlockCategorysProcedure : IProcedure
    {
        public async Task<IDataAccessDataResponse> GetResponse(HttpContext httpContext, XDocument xml)
        {
            uint userId = httpContext.IsAuthenicatedPr3User();
            if (userId > 0)
            {
                DataAccessGetMyBlockCategorysResponse response = new DataAccessGetMyBlockCategorysResponse();

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
}
