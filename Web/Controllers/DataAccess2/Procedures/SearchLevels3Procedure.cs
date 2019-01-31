using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Platform_Racing_3_Common.Level;
using Platform_Racing_3_Common.User;
using Platform_Racing_3_Web.Extensions;
using Platform_Racing_3_Web.Responses;
using Platform_Racing_3_Web.Responses.Procedures;

namespace Platform_Racing_3_Web.Controllers.DataAccess2.Procedures
{
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
}
