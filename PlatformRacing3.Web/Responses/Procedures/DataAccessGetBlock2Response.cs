using System.Collections.Generic;
using PlatformRacing3.Common.Block;

namespace PlatformRacing3.Web.Responses.Procedures
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
