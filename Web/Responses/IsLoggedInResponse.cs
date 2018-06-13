using Platform_Racing_3_Web.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Platform_Racing_3_Web.Responses
{
    public class IsLoggedInResponse
    {
        [XmlElement("IsLoggedIn")]
        public Bit IsLoggedIn { get; set; }

        [DefaultValue(0u)]
        [XmlElement("UserId")]
        public uint UserId { get; set; }
        [XmlElement("UserName")]
        public string Username { get; set; }

        public IsLoggedInResponse()
        {
            this.IsLoggedIn = false;
        }

        public IsLoggedInResponse(uint userId, string username)
        {
            this.IsLoggedIn = true;
            this.UserId = userId;
            this.Username = username;
        }
    }
}
