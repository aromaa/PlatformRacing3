using Platform_Racing_3_Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static Platform_Racing_3_Web.Responses.Procedures.DataAccessCountMyLevels2Response;

namespace Platform_Racing_3_Web.Responses.Procedures
{
    public class DataAccessCountMyLevels2Response : DataAccessDataResponse<CountMyLevelsResponse>
    {
        private DataAccessCountMyLevels2Response()
        {

        }

        public DataAccessCountMyLevels2Response(uint count)
        {
            this.Rows = new List<CountMyLevelsResponse>(1)
            {
                new CountMyLevelsResponse(count),
            };
        }
        
        public class CountMyLevelsResponse
        {
            [XmlElement("count")]
            public uint Count { get; set; }

            private CountMyLevelsResponse()
            {

            }

            public CountMyLevelsResponse(uint count)
            {
                this.Count = count;
            }
        }
    }
}
