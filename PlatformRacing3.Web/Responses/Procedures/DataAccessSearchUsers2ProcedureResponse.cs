using PlatformRacing3.Common.User;

namespace PlatformRacing3.Web.Responses.Procedures
{
	public class DataAccessSearchUsers2ProcedureResponse : DataAccessDataResponse<PlayerUserData>
    {
        private DataAccessSearchUsers2ProcedureResponse()
        {
        }

        public DataAccessSearchUsers2ProcedureResponse(IReadOnlyCollection<PlayerUserData> players)
        {
            this.Rows = new List<PlayerUserData>(players);
        }
    }
}
