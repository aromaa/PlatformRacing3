using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Platform_Racing_3_Common.Block;
using Platform_Racing_3_Web.Responses;
using Platform_Racing_3_Web.Responses.Procedures;

namespace Platform_Racing_3_Web.Controllers.DataAccess2.Procedures
{
    public class GetBlock2Procedure : IProcedure
    {
        public async Task<IDataAccessDataResponse> GetResponse(HttpContext httpContext, XDocument xml)
        {
            XElement data = xml.Element("Params");
            if (data != null)
            {
                uint blockId = (uint?)data.Element("p_block_id") ?? throw new DataAccessProcedureMissingData();

                BlockData block = await BlockManager.GetBlockAsync(blockId);
                if (block != null)
                {
                    return new DataAccessGetBlock2Response(block);
                }
                else
                {
                    return new DataAccessGetBlock2Response(BlockData.GetDeletedBlock(blockId));
                }
            }
            else
            {
                throw new DataAccessProcedureMissingData();
            }
        }
    }
}
