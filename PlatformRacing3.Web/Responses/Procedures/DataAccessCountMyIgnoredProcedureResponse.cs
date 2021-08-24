using Microsoft.AspNetCore.Http;
using Platform_Racing_3_Web.Controllers.DataAccess2.Procedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
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
            [JsonPropertyName("ignored_count")]
            public uint IgnoredCount { get; set; }

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
