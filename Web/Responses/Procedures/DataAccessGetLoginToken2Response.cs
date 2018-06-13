using Platform_Racing_3_Common.Exceptions;
using Platform_Racing_3_Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static Platform_Racing_3_Web.Responses.Procedures.DataAccessGetLoginToken2Response;

namespace Platform_Racing_3_Web.Responses.Procedures
{
    public class DataAccessGetLoginToken2Response : DataAccessDataResponse<LoginTokenResponse>
    {
        //Kinda useless warning but whatever, might remove it later
        [XmlElement("WARNING")]
        public string WARNING
        {
            get => "DO NOT GIVE THE THE LOGIN TOKEN TO ANYONE";
            set => throw new XmlSerializerCompatibleException();
        }

        private DataAccessGetLoginToken2Response()
        {

        }

        public DataAccessGetLoginToken2Response(string loginToken)
        {
            this.Rows = new List<LoginTokenResponse>(1)
            {
                new LoginTokenResponse(loginToken),
            };
        }

        public class LoginTokenResponse
        {
            [XmlElement("login_token")]
            public string Token { get; set; }

            private LoginTokenResponse()
            {

            }

            public LoginTokenResponse(string loginToken)
            {
                this.Token = loginToken;
            }
        }
    }
}
