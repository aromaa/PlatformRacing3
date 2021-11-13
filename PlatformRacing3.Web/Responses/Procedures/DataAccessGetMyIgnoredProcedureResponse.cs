using PlatformRacing3.Common.User;

namespace PlatformRacing3.Web.Responses.Procedures
{
	public class DataAccessGetMyIgnoredProcedureResponse : DataAccessDataResponse<PlayerUserData>
    {
        private DataAccessGetMyIgnoredProcedureResponse()
        {

        }

        public DataAccessGetMyIgnoredProcedureResponse(IReadOnlyCollection<PlayerUserData> players)
        {
            this.Rows = new List<PlayerUserData>(players);
        }
    }
}
