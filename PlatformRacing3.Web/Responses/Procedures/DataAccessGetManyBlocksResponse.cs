using PlatformRacing3.Common.Block;

namespace PlatformRacing3.Web.Responses.Procedures;

public class DataAccessGetManyBlocksResponse : DataAccessDataResponse<BlockData>
{
	public DataAccessGetManyBlocksResponse()
	{
		this.Rows = new List<BlockData>();
	}
        
	public void AddBlock(BlockData block)
	{
		this.Rows.Add(block);
	}
}