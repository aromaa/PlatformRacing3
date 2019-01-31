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
    public class GetMyBlocks2Procedure : IProcedure
    {
        public async Task<IDataAccessDataResponse> GetResponseAsync(HttpContext httpContext, XDocument xml)
        {
            uint userId = httpContext.IsAuthenicatedPr3User();
            if (userId > 0)
            {
                XElement data = xml.Element("Params");
                if (data != null)
                {
                    uint start = (uint?)data.Element("p_start") ?? throw new DataAccessProcedureMissingData();
                    uint count = (uint?)data.Element("p_count") ?? throw new DataAccessProcedureMissingData();
                    string category = (string)data.Element("p_category") ?? throw new DataAccessProcedureMissingData();

                    DataAccessGetMyBlocks2Response response = new DataAccessGetMyBlocks2Response(category);

                    HashSet<uint> blocks = await BlockManager.GetMyBlocksAsync(userId, category, start, count);
                    foreach(uint block in blocks)
                    {
                        response.AddBlock(block);
                    }

                    return response;
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
