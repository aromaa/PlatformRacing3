using Platform_Racing_3_Common.Level;
using Platform_Racing_3_Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Platform_Racing_3_Web.Responses.Procedures
{
    public class DataAccessGetMyLevels2Response : DataAccessDataResponse<LevelData>
    {
        private DataAccessGetMyLevels2Response()
        {

        }

        public DataAccessGetMyLevels2Response(IReadOnlyCollection<LevelData> levels)
        {
            this.Rows = new List<LevelData>(levels);
        }
    }
}
