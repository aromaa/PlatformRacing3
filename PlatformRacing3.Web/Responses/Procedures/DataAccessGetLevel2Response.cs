using Platform_Racing_3_Common.Level;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform_Racing_3_Web.Responses.Procedures
{
    public class DataAccessGetLevel2Response : DataAccessDataResponse<LevelData>
    {
        private DataAccessGetLevel2Response()
        {
        }

        public DataAccessGetLevel2Response(LevelData levelData)
        {
            this.Rows = new List<LevelData>(1)
            {
                levelData,
            };
        }
    }
}
