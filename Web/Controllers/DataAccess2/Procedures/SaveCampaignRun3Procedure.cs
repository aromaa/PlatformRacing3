using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Platform_Racing_3_Common.Campaign;
using Platform_Racing_3_Common.User;
using Platform_Racing_3_Web.Extensions;
using Platform_Racing_3_Web.Responses;
using Platform_Racing_3_Web.Responses.Procedures;

namespace Platform_Racing_3_Web.Controllers.DataAccess2.Procedures
{
    public class SaveCampaignRun3Procedure : IProcedure
    {
        public async Task<IDataAccessDataResponse> GetResponse(HttpContext httpContext, XDocument xml)
        {
            uint userId = httpContext.IsAuthenicatedPr3User();
            if (userId > 0)
            {
                XElement data = xml.Element("Params");
                if (data != null)
                {
                    uint levelId = (uint?)data.Element("p_level_id") ?? throw new DataAccessProcedureMissingData();
                    uint levelVersion = (uint?)data.Element("p_level_version") ?? throw new DataAccessProcedureMissingData();
                    string recordRun = (string)data.Element("p_recorded_run") ?? throw new DataAccessProcedureMissingData();
                    uint finishTime = (uint?)data.Element("p_finish_time") ?? throw new DataAccessProcedureMissingData();

                    JObject jsonRun = null;
                    using (MemoryStream compressedMemoryStream = new MemoryStream(Convert.FromBase64String(recordRun)))
                    {
                        using (InflaterInputStream inflater = new InflaterInputStream(compressedMemoryStream))
                        {
                            using (MemoryStream uncompressedMemoryStream = new MemoryStream())
                            {
                                inflater.CopyTo(uncompressedMemoryStream);

                                jsonRun = JsonConvert.DeserializeObject<JObject>(Encoding.UTF8.GetString(uncompressedMemoryStream.ToArray()));
                            }
                        }
                    }

                    if (jsonRun != null)
                    {
                        PlayerUserData playerUserData = await UserManager.TryGetUserDataByIdAsync(userId);
                        if (!jsonRun.TryGetValue("userName", out JToken usernameJson))
                        {
                            return new DataAccessErrorResponse("Missing username");
                        }
                        else if ((string)usernameJson != playerUserData.Username)
                        {
                            return new DataAccessErrorResponse("Invalid username");
                        }

                        if (!jsonRun.TryGetValue("playbackFreq", out JToken playbackFreqJson))
                        {
                            return new DataAccessErrorResponse("Missing playback freq");
                        }
                        else if ((uint)playbackFreqJson != 250)
                        {
                            return new DataAccessErrorResponse("Unsupported playback freq");
                        }

                        if (!jsonRun.TryGetValue("updateArray", out JToken updateArrayJson))
                        {
                            return new DataAccessErrorResponse("Missing updates");
                        }
                        else
                        {
                            //We should do some kinda validation here but lol!
                        }

                        if (!jsonRun.TryGetValue("hat", out _))
                        {
                            return new DataAccessErrorResponse("Missing hat");
                        }

                        if (!jsonRun.TryGetValue("hatColor", out _))
                        {
                            return new DataAccessErrorResponse("Missing hat color");
                        }

                        if (!jsonRun.TryGetValue("head", out _))
                        {
                            return new DataAccessErrorResponse("Missing head");
                        }

                        if (!jsonRun.TryGetValue("headColor", out _))
                        {
                            return new DataAccessErrorResponse("Missing head color");
                        }

                        if (!jsonRun.TryGetValue("body", out _))
                        {
                            return new DataAccessErrorResponse("Missing body");
                        }

                        if (!jsonRun.TryGetValue("bodyColor", out _))
                        {
                            return new DataAccessErrorResponse("Missing body color");
                        }

                        if (!jsonRun.TryGetValue("feet", out _))
                        {
                            return new DataAccessErrorResponse("Missing feet");
                        }

                        if (!jsonRun.TryGetValue("feetColor", out _))
                        {
                            return new DataAccessErrorResponse("Missing feet color");
                        }

                        await CampaignManager.SaveCampaignRun(userId, levelId, levelVersion, recordRun, finishTime);

                        return new DataAccessSaveCampaignRun3Response();
                    }
                    else
                    {
                        throw new DataAccessProcedureMissingData();
                    }
                }
                else
                {
                    throw new DataAccessProcedureMissingData();
                }
            }
            else
            {
                return new DataAccessErrorResponse("You are not logged in!");
            }
        }
    }
}
