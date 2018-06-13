using Platform_Racing_3_Common.Exceptions;
using Platform_Racing_3_Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Platform_Racing_3_Web.Responses
{
    public abstract class DataAccessDataResponse<T> : IDataAccessDataResponse
    {
        [XmlElement("DataRequestID")]
        public uint DataRequestId { get; set; }

        [XmlElement("Row")]
        public List<T> Rows { get; set; }

        [XmlElement("NumRows")]
        public uint NumRows
        {
            get => (uint)this.Rows.Count;
            set => throw new XmlSerializerCompatibleException();
        }
        
        public DataAccessDataResponse()
        {

        }

        public DataAccessDataResponse(uint requestId)
        {
            this.DataRequestId = requestId;
        }
    }
}
