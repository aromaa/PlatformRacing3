using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Platform_Racing_3_Common.User;
using Platform_Racing_3_Web.Responses;
using Platform_Racing_3_Web.Responses.Procedures;

namespace Platform_Racing_3_Web.Controllers.DataAccess2.Procedures
{
    public class SearchUsers2Procedure : IProcedure
    {
        public async Task<IDataAccessDataResponse> GetResponseAsync(HttpContext httpContext, XDocument xml)
        {
            XElement data = xml.Element("Params");
            if (data != null)
            {
                string name = (string)data.Element("p_name") ?? throw new DataAccessProcedureMissingData();

                IReadOnlyCollection<PlayerUserData> users = await UserManager.SearchUsers(name);

                return new DataAccessSearchUsers2ProcedureResponse(users);
            }
            else
            {
                throw new DataAccessProcedureMissingData();
            }
        }
    }
}
