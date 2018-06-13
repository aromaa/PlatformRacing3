using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Platform_Racing_3_Web.Responses
{
    public class DataAccessErrorResponse : IDataAccessDataResponse
    {
        [XmlElement("DataRequestID")]
        public uint DataRequestId { get; set; }
        
        [XmlElement("Error")]
        public string Error { get; set; }

        private DataAccessErrorResponse()
        {
        }

        public DataAccessErrorResponse(string error)
        {
            this.Error = error;
        }

        public DataAccessErrorResponse(uint requestId, string error)
        {
            this.DataRequestId = requestId;
            this.Error = error;
        }
    }
}
