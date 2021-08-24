using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static Platform_Racing_3_Web.Responses.Procedures.DataAccessCountMyFriendsProcedureResponse;

namespace Platform_Racing_3_Web.Responses.Procedures
{
    public class DataAccessCountMyFriendsProcedureResponse : DataAccessDataResponse<FriendsCountResponse>
    {
        private DataAccessCountMyFriendsProcedureResponse()
        {

        }

        public DataAccessCountMyFriendsProcedureResponse(uint friendsCount)
        {
            this.Rows = new List<FriendsCountResponse>()
            {
                new FriendsCountResponse(friendsCount),
            };
        }

        public class FriendsCountResponse
        {
            private FriendsCountResponse()
            {

            }

            [XmlElement("friend_count")]
            public uint FriendsCount { get; set; }

            public FriendsCountResponse(uint friendsCount)
            {
                this.FriendsCount = friendsCount;
            }
        }
    }
}
