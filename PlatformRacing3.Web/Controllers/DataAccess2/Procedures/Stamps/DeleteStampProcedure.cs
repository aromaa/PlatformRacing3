using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Platform_Racing_3_Common.Stamp;
using Platform_Racing_3_Web.Extensions;
using Platform_Racing_3_Web.Responses;
using Platform_Racing_3_Web.Responses.Procedures;
using Platform_Racing_3_Web.Responses.Procedures.Stamps;

namespace Platform_Racing_3_Web.Controllers.DataAccess2.Procedures.Stamps
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
