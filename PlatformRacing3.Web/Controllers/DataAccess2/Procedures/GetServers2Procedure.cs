using Microsoft.AspNetCore.Http;
using Platform_Racing_3_Common.Server;
using Platform_Racing_3_Web.Responses;
using Platform_Racing_3_Web.Responses.Procedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Platform_Racing_3_Web.Controllers.DataAccess2.Procedures
{
    public class GetServers2Procedure : IProcedure
    {
        private readonly ServerManager serverManager;

        public GetServers2Procedure(ServerManager serverManager)
        {
            this.serverManager = serverManager;
        }

        public Task<IDataAccessDataResponse> GetResponseAsync(HttpContext httpContext, XDocument xml)
        {
            return Task.FromResult<IDataAccessDataResponse>(new DataAccessGetServers2Response(this.serverManager.GetServers()));
        }
    }
}
