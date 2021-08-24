using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using PlatformRacing3.Web.Responses;

namespace PlatformRacing3.Web.Controllers.DataAccess2.Procedures
{
    public interface IProcedure
    {
        Task<IDataAccessDataResponse> GetResponseAsync(HttpContext httpContext, XDocument xml);
    }
}
