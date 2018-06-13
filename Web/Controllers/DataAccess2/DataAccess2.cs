using log4net;
using Microsoft.AspNetCore.Mvc;
using Platform_Racing_3_Web.Controllers.DataAccess2.Procedures;
using Platform_Racing_3_Web.Responses;
using Platform_Racing_3_Web.Responses.Procedures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Platform_Racing_3_Web.Controllers.DataAccess2
{
    [Route("dataaccess2")]
    [Produces("text/xml")]
    public class DataAccess2 : Controller
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly byte[] KEY = Encoding.UTF8.GetBytes("012345678910ABCD");
        private static readonly IReadOnlyDictionary<string, IProcedure> Procedures = new Dictionary<string, IProcedure>()
        {
            { "GetServers2", new GetServers2Procedure() },
            { "GetLoginToken2", new GetLoginToken2Procedure() },
            { "SaveLevel4",  new SaveLevel4Procedure() },
            { "CountMyLevels2", new CountMyLevels2Procedure() },
            { "GetMyLevels2", new GetMyLevels2Procedure() },
            { "GetLevel2", new GetLevel2Procedure() },
            { "DeleteLevel2", new DeleteLevel2Procedure() },
            { "SaveBlock4", new SaveBlock4Procedure() },
            { "GetMyBlockCategorys", new GetMyBlockCategorysProcedure() },
            { "CountMyBlocks2", new CountMyBlocks2Procedure() },
            { "GetMyBlocks2", new GetMyBlocks2Procedure() },
            { "GetBlock2", new GetBlock2Procedure() },
            { "GetManyBlocks", new GetManyBlocksProcedure() },
            { "DeleteBlock2", new DeleteBlock2Procedure() },
            { "CountMyFriends", new CountMyFriendsProcedure() },
            { "GetMyFriends", new GetMyFriendsProcedure() },
            { "CountMyIgnored", new CountMyIgnoredProcedure() },
            { "GetMyIgnored", new GetMyIgnoredProcedure() },
            { "SearchUsers2", new SearchUsers2Procedure() },
            { "SearchLevels3", new SearchLevels3Procedure() },
            { "GetLockedLevel", new GetLockedLevelProcedure() },
            { "SaveCampaignRun3", new SaveCampaignRun3Procedure() },
            { "GetMyFriendsFastestRuns", new GetMyFriendsFastestRunsProcedure() },
        };

        [HttpPost]
        public async Task<object> DataAccessAsync([FromQuery] uint id, [FromForm] uint dataRequestID, [FromForm(Name = "gameId")] string gameIdEncoded, [FromForm(Name = "storedProcID")] string storedProcIdEncoded, [FromForm(Name = "storedProcedureName")] string storedProcedureNameEncoded, [FromForm(Name = "parametersXML")] string parametersXmlEncoded)
        {
            if (id == dataRequestID)
            {
                if (gameIdEncoded != null && storedProcIdEncoded != null && storedProcedureNameEncoded != null && parametersXmlEncoded != null)
                {
                    byte[] storedProcId = Convert.FromBase64String(storedProcIdEncoded);

                    string gameId = this.DecryptData(gameIdEncoded, storedProcId, DataAccess2.KEY);
                    if (gameId == "f1c25e3bd3523110394b5659c68d8092")
                    {
                        string storedProcedureName = this.DecryptData(storedProcedureNameEncoded, storedProcId, DataAccess2.KEY);
                        if (DataAccess2.Procedures.TryGetValue(storedProcedureName, out IProcedure procedure))
                        {
                            XDocument xml = XDocument.Parse(this.DecryptData(parametersXmlEncoded, storedProcId, DataAccess2.KEY));

                            try
                            {
                                IDataAccessDataResponse response = await procedure.GetResponse(this.HttpContext, xml);
                                response.DataRequestId = dataRequestID;
                                return response;
                            }
                            catch (DataAccessProcedureMissingData)
                            {
                                return new DataAccessErrorResponse(dataRequestID, "Invalid request, procedure was missing required data");
                            }
                            catch(Exception ex)
                            {
                                DataAccess2.Logger.Error("Failed to execute procedure", ex);

                                return new DataAccessErrorResponse(dataRequestID, "Critical error while executing procedure");
                            }
                        }
                        else
                        {
                            return new DataAccessErrorResponse(dataRequestID, "No procedure found by the name");
                        }
                    }
                }
            }

            return this.BadRequest();
        }

        private string DecryptData(string data, byte[] iv, byte[] key)
        {
            byte[] bytes = Convert.FromBase64String(data);

            using (Rijndael crypt = Rijndael.Create())
            {
                crypt.Mode = CipherMode.CBC;
                crypt.KeySize = 128;
                crypt.Padding = PaddingMode.Zeros;

                crypt.IV = iv;
                crypt.Key = key;

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (ICryptoTransform decryptor = crypt.CreateDecryptor())
                    {
                        using (CryptoStream stream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write))
                        {
                            stream.Write(bytes, 0, bytes.Length);
                        }
                    }

                    return Encoding.UTF8.GetString(memoryStream.ToArray()).Trim('\0'); //Trim to remove extra null bytes
                }
            }
        }
    }
}
