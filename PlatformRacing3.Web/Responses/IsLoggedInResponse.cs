using System.ComponentModel;
using System.Xml.Serialization;
using PlatformRacing3.Web.Utils;

namespace PlatformRacing3.Web.Responses;

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