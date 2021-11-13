using PlatformRacing3.Common.Server;

namespace PlatformRacing3.Web.Responses.Procedures;

public class DataAccessGetServers2Response : DataAccessDataResponse<ServerDetails>
{
	private DataAccessGetServers2Response()
	{
	}

	public DataAccessGetServers2Response(IReadOnlyCollection<ServerDetails> servers)
	{
		this.Rows = new List<ServerDetails>(servers);
	}
}