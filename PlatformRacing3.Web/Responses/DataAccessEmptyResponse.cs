using Platform_Racing_3_Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Platform_Racing_3_Web.Responses
{
    public class DataAccessEmptyResponse : IDataAccessDataResponse
    {
        [XmlElement("DataRequestID")]
        public uint DataRequestId { get; set; }

        [XmlElement("Row")]
        public object[] Rows { get; set; } = Array.Empty<object>();

        [XmlElement("NumRows")]
        public uint NumRows
        {
            get => 0;
            set => throw new XmlSerializerCompatibleException();
        }

        private DataAccessEmptyResponse()
        {

        }

        public DataAccessEmptyResponse(uint requestId)
        {
            this.DataRequestId = requestId;
        }
    }
}
