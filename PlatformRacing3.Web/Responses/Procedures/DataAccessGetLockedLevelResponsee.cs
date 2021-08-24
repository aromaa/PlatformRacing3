using Platform_Racing_3_Common.Level;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform_Racing_3_Web.Responses.Procedures
{
    public class DataAccessGetLockedLevelResponsee : DataAccessDataResponse<LevelData>
    {
        private DataAccessGetLockedLevelResponsee()
        {
        }

        public DataAccessGetLockedLevelResponsee(LevelData levelData)
        {
            this.Rows = new List<LevelData>(1)
            {
                levelData,
            };
        }
    }
}
