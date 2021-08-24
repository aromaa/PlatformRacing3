using System.Collections.Generic;
using PlatformRacing3.Common.Level;

namespace PlatformRacing3.Web.Responses.Procedures
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
