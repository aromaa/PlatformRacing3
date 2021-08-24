using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using PlatformRacing3.Common.Stamp;
using PlatformRacing3.Web.Controllers.DataAccess2.Procedures.Exceptions;
using PlatformRacing3.Web.Extensions;
using PlatformRacing3.Web.Responses;
using PlatformRacing3.Web.Responses.Procedures.Stamps;

namespace PlatformRacing3.Web.Controllers.DataAccess2.Procedures.Stamps
{
    public class DeleteStampProcedure : IProcedure
    {
        public async Task<IDataAccessDataResponse> GetResponseAsync(HttpContext httpContext, XDocument xml)
        {
            uint userId = httpContext.IsAuthenicatedPr3User();
            if (userId > 0)
            {
                XElement data = xml.Element("Params");
                if (data != null)
                {
                    uint stampId = (uint?)data.Element("p_stamp_id") ?? throw new DataAccessProcedureMissingData();

                    await StampManager.DeleteStampAsync(stampId, userId);

                    return new DataAccessDeleteStampResponse();
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
}
