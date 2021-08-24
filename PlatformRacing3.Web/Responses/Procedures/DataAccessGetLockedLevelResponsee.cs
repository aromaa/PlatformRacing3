using System.Collections.Generic;
using PlatformRacing3.Common.Level;

namespace PlatformRacing3.Web.Responses.Procedures
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
