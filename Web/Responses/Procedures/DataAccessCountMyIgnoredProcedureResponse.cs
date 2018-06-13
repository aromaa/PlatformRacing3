using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Platform_Racing_3_Web.Controllers.DataAccess2.Procedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Platform_Racing_3_Web.Responses.Procedures.DataAccessCountMyIgnoredProcedureResponse;

namespace Platform_Racing_3_Web.Responses.Procedures
{
    public class DataAccessCountMyIgnoredProcedureResponse : DataAccessDataResponse<CountMyIgnoredResponse>
    {
        private DataAccessCountMyIgnoredProcedureResponse()
        {

        }

        public DataAccessCountMyIgnoredProcedureResponse(uint ignoredCount)
        {
            this.Rows = new List<CountMyIgnoredResponse>()
            {
                new CountMyIgnoredResponse(ignoredCount),
            };
        }

        public class CountMyIgnoredResponse
        {
            [JsonProperty("ignored_count")]
            internal uint IgnoredCount { get; set; }

            private CountMyIgnoredResponse()
            {

            }

            public CountMyIgnoredResponse(uint ignoredCount)
            {
                this.IgnoredCount = ignoredCount;
            }
        }
    }
}
