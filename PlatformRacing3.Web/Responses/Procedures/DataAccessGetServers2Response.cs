using Platform_Racing_3_Common.Server;
using Platform_Racing_3_Web.Controllers.DataAccess2.Procedures;
using Platform_Racing_3_Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Platform_Racing_3_Web.Responses.Procedures
{
    public class DataAccessGetServers2Response : DataAccessDataResponse<ServerDetails>
    {
        private DataAccessGetServers2Response()
        {
        }

        public DataAccessGetServers2Response(IReadOnlyCollection<ServerDetails> servers)
        {
            this.Rows = new List<ServerDetails>(servers);
        }
    }
}
