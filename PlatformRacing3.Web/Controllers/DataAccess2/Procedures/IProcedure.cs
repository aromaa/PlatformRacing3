using System.Xml.Linq;
using PlatformRacing3.Web.Responses;

namespace PlatformRacing3.Web.Controllers.DataAccess2.Procedures;

public interface IProcedure
{
	Task<IDataAccessDataResponse> GetResponseAsync(HttpContext httpContext, XDocument xml);
}