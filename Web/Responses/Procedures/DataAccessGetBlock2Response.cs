using Platform_Racing_3_Common.Block;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform_Racing_3_Web.Responses.Procedures
{
    public class DataAccessGetBlock2Response : DataAccessDataResponse<BlockData>
    {
        public DataAccessGetBlock2Response()
        {
            this.Rows = new List<BlockData>();
        }

        public DataAccessGetBlock2Response(BlockData blockData)
        {
            this.Rows = new List<BlockData>()
            {
                blockData
            };
        }
    }
}
