using System.Xml.Serialization;
using PlatformRacing3.Common.Exceptions;

namespace PlatformRacing3.Web.Responses
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
