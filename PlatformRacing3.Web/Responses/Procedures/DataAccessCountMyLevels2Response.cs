using System.Xml.Serialization;

namespace PlatformRacing3.Web.Responses.Procedures;

public class DataAccessCountMyLevels2Response : DataAccessDataResponse<DataAccessCountMyLevels2Response.CountMyLevelsResponse>
{
	private DataAccessCountMyLevels2Response()
	{

	}

	public DataAccessCountMyLevels2Response(uint count)
	{
		this.Rows = new List<CountMyLevelsResponse>(1)
		{
			new CountMyLevelsResponse(count),
		};
	}
        
	public class CountMyLevelsResponse
	{
		[XmlElement("count")]
		public uint Count { get; set; }

		private CountMyLevelsResponse()
		{

		}

		public CountMyLevelsResponse(uint count)
		{
			this.Count = count;
		}
	}
}