using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PlatformRacing3.Web.Responses.Procedures
{
    public class DataAccessCountMyIgnoredProcedureResponse : DataAccessDataResponse<DataAccessCountMyIgnoredProcedureResponse.CountMyIgnoredResponse>
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
