using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform_Racing_3_Web.Responses.Procedures
{
    public class DataAccessDeleteBlockResponse : DataAccessDataResponse<bool>
    {
        public DataAccessDeleteBlockResponse()
        {
            this.Rows = new List<bool>(0);
        }
    }
}
