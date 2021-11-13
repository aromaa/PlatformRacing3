using PlatformRacing3.Common.Level;

namespace PlatformRacing3.Web.Responses.Procedures;

public class DataAccessSearchLevels3Response : DataAccessDataResponse<LevelData>
{
	private DataAccessSearchLevels3Response()
	{
	}

	public DataAccessSearchLevels3Response(IReadOnlyCollection<LevelData> levels)
	{
		this.Rows = new List<LevelData>(levels);
	}
}