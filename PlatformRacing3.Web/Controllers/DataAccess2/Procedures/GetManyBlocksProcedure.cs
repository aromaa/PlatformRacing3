using System.Xml.Linq;
using PlatformRacing3.Common.Block;
using PlatformRacing3.Web.Controllers.DataAccess2.Procedures.Exceptions;
using PlatformRacing3.Web.Responses;
using PlatformRacing3.Web.Responses.Procedures;

namespace PlatformRacing3.Web.Controllers.DataAccess2.Procedures
{
	public class GetManyBlocksProcedure : IProcedure
    {
        public async Task<IDataAccessDataResponse> GetResponseAsync(HttpContext httpContext, XDocument xml)
        {
            XElement data = xml.Element("Params");
            if (data != null)
            {
                uint[] blockIds = ((string)data.Element("p_block_array") ?? throw new DataAccessProcedureMissingData()).Split(',').Select((b) => uint.Parse(b)).ToArray();
                if (blockIds.Length > 0)
                {
                    DataAccessGetManyBlocksResponse response = new();

                    List<BlockData> blocks = await BlockManager.GetBlocksAsync(blockIds);
                    foreach(BlockData block in blocks)
                    {
                        response.AddBlock(block);
                    }

                    if (blocks.Count != blockIds.Length)
                    {
                        foreach(uint blockId in blockIds)
                        {
                            if (!blocks.Any((b) => b.Id == blockId))
                            {
                                response.AddBlock(BlockData.GetDeletedBlock(blockId));
                            }
                        }
                    }

                    return response;
                }
                else
                {
                    return new DataAccessGetManyBlocksResponse();
                }
            }
            else
            {
                throw new DataAccessProcedureMissingData();
            }
        }
    }
}
