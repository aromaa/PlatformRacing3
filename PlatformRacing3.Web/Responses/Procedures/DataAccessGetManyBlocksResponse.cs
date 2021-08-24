using Platform_Racing_3_Common.Block;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform_Racing_3_Web.Responses.Procedures
{
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
}
