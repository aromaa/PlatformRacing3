using Platform_Racing_3_Common.Stamp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static Platform_Racing_3_Web.Responses.Procedures.Stamps.DataAccessGetMyStampsResponse;

namespace Platform_Racing_3_Web.Responses.Procedures.Stamps
{
    public class DataAccessGetMyStampsResponse : DataAccessDataResponse<MyStampData>
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
