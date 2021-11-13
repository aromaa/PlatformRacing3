using System.Xml.Serialization;

namespace PlatformRacing3.Web.Responses.Procedures;

public class DataAccessCountMyFriendsProcedureResponse : DataAccessDataResponse<DataAccessCountMyFriendsProcedureResponse.FriendsCountResponse>
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