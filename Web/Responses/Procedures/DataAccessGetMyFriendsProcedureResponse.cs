using Platform_Racing_3_Common.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform_Racing_3_Web.Responses.Procedures
{
    public class DataAccessGetMyFriendsProcedureResponse : DataAccessDataResponse<PlayerUserData>
    {
        private DataAccessGetMyFriendsProcedureResponse()
        {

        }

        public DataAccessGetMyFriendsProcedureResponse(IReadOnlyCollection<PlayerUserData> players)
        {
            this.Rows = new List<PlayerUserData>(players);
        }
    }
}
