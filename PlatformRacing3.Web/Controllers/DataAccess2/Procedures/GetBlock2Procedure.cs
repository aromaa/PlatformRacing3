using System.Xml.Linq;
using PlatformRacing3.Common.Block;
using PlatformRacing3.Web.Controllers.DataAccess2.Procedures.Exceptions;
using PlatformRacing3.Web.Responses;
using PlatformRacing3.Web.Responses.Procedures;

namespace PlatformRacing3.Web.Controllers.DataAccess2.Procedures
{
	public class GetBlock2Procedure : IProcedure
    {
        public async Task<IDataAccessDataResponse> GetResponseAsync(HttpContext httpContext, XDocument xml)
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
