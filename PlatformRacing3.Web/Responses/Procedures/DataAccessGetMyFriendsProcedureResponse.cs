using PlatformRacing3.Common.User;

namespace PlatformRacing3.Web.Responses.Procedures;

public class DataAccessGetMyFriendsProcedureResponse : DataAccessDataResponse<PlayerUserData>
{
	private DataAccessGetMyFriendsProcedureResponse()
	{

	}

	public DataAccessGetMyFriendsProcedureResponse(IReadOnlyCollection<PlayerUserData> players)
	{
		this.Rows = new List<PlayerUserData>(players);
	}
}