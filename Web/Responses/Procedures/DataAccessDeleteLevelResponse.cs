using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform_Racing_3_Web.Responses.Procedures
{
    public class DataAccessDeleteLevelResponse : DataAccessDataResponse<bool>
    {
        public DataAccessDeleteLevelResponse()
        {
            this.Rows = new List<bool>(0);
        }
    }
}
