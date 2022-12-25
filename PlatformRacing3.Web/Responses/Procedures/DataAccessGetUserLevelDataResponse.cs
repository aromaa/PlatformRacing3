using System.Xml.Serialization;

namespace PlatformRacing3.Web.Responses.Procedures;

public class DataAccessGetUserLevelDataResponse : DataAccessDataResponse<DataAccessGetUserLevelDataResponse.LevelDataResponse>
{
	public DataAccessGetUserLevelDataResponse()
	{
	}

	public DataAccessGetUserLevelDataResponse(string levelData)
	{
		this.Rows = new List<LevelDataResponse>(1)
		{
			new(levelData)
		};
	}

	public class LevelDataResponse
	{
		[XmlElement("level_data")]
		public string LevelData { get; set; }

		public LevelDataResponse()
		{

		}

		public LevelDataResponse(string levelData)
		{
			this.LevelData = levelData;
		}
	}
}
