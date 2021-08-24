using System.Collections.Generic;

namespace PlatformRacing3.Web.Responses.Procedures
{
    public class DataAccessDeleteBlockResponse : DataAccessDataResponse<bool>
    {
        public DataAccessDeleteBlockResponse()
        {
            this.Rows = new List<bool>(0);
        }
    }
}
