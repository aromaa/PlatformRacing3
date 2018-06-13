using Microsoft.AspNetCore.Http;
using Platform_Racing_3_Web.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Platform_Racing_3_Web.Controllers.DataAccess2.Procedures
{
    public interface IProcedure
    {
        Task<IDataAccessDataResponse> GetResponse(HttpContext httpContext, XDocument xml);
    }
}
