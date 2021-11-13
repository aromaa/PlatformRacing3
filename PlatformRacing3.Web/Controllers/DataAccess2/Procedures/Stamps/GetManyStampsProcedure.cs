using System.Xml.Linq;
using PlatformRacing3.Common.Stamp;
using PlatformRacing3.Web.Controllers.DataAccess2.Procedures.Exceptions;
using PlatformRacing3.Web.Responses;
using PlatformRacing3.Web.Responses.Procedures.Stamps;

namespace PlatformRacing3.Web.Controllers.DataAccess2.Procedures.Stamps
{
	public class GetManyStampsProcedure : IProcedure
    {
        public async Task<IDataAccessDataResponse> GetResponseAsync(HttpContext httpContext, XDocument xml)
        {
            XElement data = xml.Element("Params");
            if (data != null)
            {
                uint[] stampIds = ((string)data.Element("p_stamp_array") ?? throw new DataAccessProcedureMissingData()).Split(',').Select((b) => uint.Parse(b)).ToArray();
                if (stampIds.Length > 0)
                {
                    DataAccessGetManyStampsResponse response = new();

                    IList<StampData> stamps = await StampManager.GetStampsAsync(stampIds);
                    foreach (StampData block in stamps)
                    {
                        response.AddStamp(block);
                    }

                    if (stamps.Count != stampIds.Length)
                    {
                        foreach (uint blockId in stampIds)
                        {
                            if (!stamps.Any((b) => b.Id == blockId))
                            {
                                response.AddStamp(StampData.GetDeletedStamp(blockId));
                            }
                        }
                    }

                    return response;
                }
                else
                {
                    return new DataAccessGetManyStampsResponse();
                }
            }
            else
            {
                throw new DataAccessProcedureMissingData();
            }
        }
    }
}
