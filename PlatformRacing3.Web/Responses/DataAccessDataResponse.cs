using System.Xml.Serialization;
using PlatformRacing3.Common.Exceptions;

namespace PlatformRacing3.Web.Responses
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
            get => (uint?)this.Rows?.Count ?? 0u;
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
