using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Platform_Racing_3_Common.Stamp;
using Platform_Racing_3_Web.Responses;
using Platform_Racing_3_Web.Responses.Procedures.Stamps;

namespace Platform_Racing_3_Web.Controllers.DataAccess2.Procedures.Stamps
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
