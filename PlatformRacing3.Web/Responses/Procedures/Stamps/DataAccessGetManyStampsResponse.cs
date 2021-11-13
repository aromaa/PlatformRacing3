using PlatformRacing3.Common.Stamp;

namespace PlatformRacing3.Web.Responses.Procedures.Stamps
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
