using Platform_Racing_3_Common.Level;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform_Racing_3_Web.Responses.Procedures
{
    public class DataAccessSearchLevels3Response : DataAccessDataResponse<LevelData>
    {
        private DataAccessSearchLevels3Response()
        {
        }

        public DataAccessSearchLevels3Response(IReadOnlyCollection<LevelData> levels)
        {
            this.Rows = new List<LevelData>(levels);
        }
    }
}
