using System.Xml.Linq;
using PlatformRacing3.Common.Level;
using PlatformRacing3.Web.Controllers.DataAccess2.Procedures.Exceptions;
using PlatformRacing3.Web.Responses;
using PlatformRacing3.Web.Responses.Procedures;

namespace PlatformRacing3.Web.Controllers.DataAccess2.Procedures
{
	public class GetLevel2Procedure : IProcedure
    {
        public async Task<IDataAccessDataResponse> GetResponseAsync(HttpContext httpContext, XDocument xml)
        {
            XElement data = xml.Element("Params");
            if (data != null)
            {
                uint levelId = (uint?)data.Element("p_level_id") ?? throw new DataAccessProcedureMissingData();

                LevelData levelData = await LevelManager.GetLevelDataAsync(levelId);
                if (levelData != null)
                {
                    /*uint userId = httpContext.IsAuthenicatedPr3User();
                    if (levelData.Publish || (userId > 0 && levelData.AuthorUserId == userId))
                    {
                        return new DataAccessGetLevel2Response(levelData);
                    }
                    else
                    {
                        return new DataAccessErrorResponse("You may not access unpublished levels!");
                    }*/

                    //TODO: Fix this, only allow accessing unpublished levels if going try matchlisting, can be easily done using redis tokens etc

                    return new DataAccessGetLevel2Response(levelData);
                }
                else
                {
                    return new DataAccessErrorResponse("Level was not found");
                }
            }
            else
            {
                throw new DataAccessProcedureMissingData();
            }
        }
    }
}
