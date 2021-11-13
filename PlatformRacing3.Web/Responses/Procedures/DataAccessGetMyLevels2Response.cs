using PlatformRacing3.Common.Level;

namespace PlatformRacing3.Web.Responses.Procedures
{
	public class DataAccessGetMyLevels2Response : DataAccessDataResponse<LevelData>
    {
        private DataAccessGetMyLevels2Response()
        {

        }

        public DataAccessGetMyLevels2Response(IReadOnlyCollection<LevelData> levels)
        {
            this.Rows = new List<LevelData>(levels);
        }
    }
}
