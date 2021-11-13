namespace PlatformRacing3.Web.Responses.Procedures;

public class DataAccessDeleteLevelResponse : DataAccessDataResponse<bool>
{
	public DataAccessDeleteLevelResponse()
	{
		this.Rows = new List<bool>(0);
	}
}