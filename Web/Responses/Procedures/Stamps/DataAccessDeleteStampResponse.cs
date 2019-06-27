using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform_Racing_3_Web.Responses.Procedures.Stamps
{
    public class DataAccessDeleteStampResponse : DataAccessDataResponse<bool>
    {
        public DataAccessDeleteStampResponse()
        {
            this.Rows = new List<bool>(0);
        }
    }
}
