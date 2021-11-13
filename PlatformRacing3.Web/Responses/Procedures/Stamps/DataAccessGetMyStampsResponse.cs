using System.Xml.Serialization;

namespace PlatformRacing3.Web.Responses.Procedures.Stamps
{
	public class DataAccessGetMyStampsResponse : DataAccessDataResponse<DataAccessGetMyStampsResponse.MyStampData>
    {
        [XmlElement("category")]
        public string Category { get; set; }

        private DataAccessGetMyStampsResponse()
        {

        }

        public DataAccessGetMyStampsResponse(string category)
        {
            this.Rows = new List<MyStampData>();

            this.Category = category;
        }

        public void AddStamp(uint stampId)
        {
            this.Rows.Add(new MyStampData(stampId));
        }

        public class MyStampData
        {
            [XmlElement("stamp_id")]
            public uint StampId { get; set; }

            private MyStampData()
            {

            }

            public MyStampData(uint stampId)
            {
                this.StampId = stampId;
            }
        }
    }
}
