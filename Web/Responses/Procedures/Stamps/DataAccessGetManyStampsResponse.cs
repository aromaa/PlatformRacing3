using Platform_Racing_3_Common.Stamp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform_Racing_3_Web.Responses.Procedures.Stamps
{
    public class DataAccessGetManyStampsResponse : DataAccessDataResponse<StampData>
    {
        public DataAccessGetManyStampsResponse()
        {
            this.Rows = new List<StampData>();
        }

        public void AddStamp(StampData stamp)
        {
            this.Rows.Add(stamp);
        }
    }
}
