using Microsoft.AspNetCore.Mvc;
using Platform_Racing_3_Web.Controllers.DataAccess2.Procedures;
using Platform_Racing_3_Web.Controllers.DataAccess2.Procedures.Stamps;
using Platform_Racing_3_Web.Responses;
using Platform_Racing_3_Web.Responses.Procedures;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Platform_Racing_3_Common.Server;
using Platform_Racing_3_Web.Extensions;
using Platform_Racing_3_Common.User;
using Platform_Racing_3_Web.Config;
using Platform_Racing_3_Web.Utils;

namespace Platform_Racing_3_Web.Controllers.DataAccess2
{
    [ApiController]
    [Route("dataaccess2")]
    [Produces("text/xml")]
    public class DataAccess2 : ControllerBase
    {
        private static readonly byte[] DEFAULT_KEY = Encoding.UTF8.GetBytes("012345678910ABCD");

        private static readonly IReadOnlyDictionary<string, ConcurrentDictionary<byte[], byte>> KEYS = new Dictionary<string, ConcurrentDictionary<byte[], byte>>()
        {
            {
                "Android",
                new ConcurrentDictionary<byte[], byte>()
            },
            {
                "iOS",
                new ConcurrentDictionary<byte[], byte>()
            },
            {
                "Browser",
                new ConcurrentDictionary<byte[], byte>()
            },
            {
                "Standalone",
                new ConcurrentDictionary<byte[], byte>()
            }
        };

        internal static void Init(WebConfig webConfig)
        {
            if (string.IsNullOrWhiteSpace(webConfig.GamePath))
            {
                return;
            }

            PhysicalFileProvider val = new(webConfig.GamePath, ExclusionFilters.Sensitive)
            {
                UsePollingFileWatcher = true,
                UseActivePolling = true
            };
            
            RefreshSwfs();
            RegisterWatch();

            void RefreshSwfs()
            {
                Console.WriteLine("Refreshing SWFs");

                Parallel.ForEach(new DirectoryInfo(webConfig.GamePath).GetFiles().OrderBy(f => f.LastWriteTime), async swf =>
                {
                    byte[] bytes = await System.IO.File.ReadAllBytesAsync(swf.FullName);

                    DataAccess2.CalcHashAndAdd(bytes);
                });
            }

            void RegisterWatch()
            {
	            IChangeToken token = val.Watch("*.swf");
	            token.RegisterChangeCallback(state =>
	            {
		            RefreshSwfs();
		            RegisterWatch();
                }, null);
            }
        }

        private static void CalcHashAndAdd(byte[] bytes)
        {
            try
            {
                int crc = DataAccess2.GetCrc32(bytes.AsSpan(3));
                string s = DataAccess2.GetHash(crc);

                byte[] bytes2 = Encoding.UTF8.GetBytes(s);
                if (bytes2.Length == 16)
                {
                    DataAccess2.KEYS["Android"].TryAdd(bytes2, 0);
                    DataAccess2.KEYS["Browser"].TryAdd(bytes2, 0);
                    DataAccess2.KEYS["Standalone"].TryAdd(bytes2, 0);
                }
            }
            catch
            {
            }
        }

        private static int GetCrc32(ReadOnlySpan<byte> span)
        {
            using (MemoryStream memoryStream = new(span[5..].ToArray()))
            {
	            //TODO: Read zlib header, .NET 6 has ZlibStream, switch to that and remove this workaround
                memoryStream.Position = 2;

                using (DeflateStream val = new(memoryStream, CompressionMode.Decompress))
                {
                    using (MemoryStream memoryStream2 = new())
                    {
                        memoryStream2.Write(Encoding.UTF8.GetBytes("FWS"));
                        memoryStream2.Write(span[..5]);

                        val.CopyTo(memoryStream2);

                        return DataAccess2.CalculateCrc32(memoryStream2.ToArray());
                    }
                }
            }
        }

        private static int CalculateCrc32(byte[] bytes)
        {
            int[] array = DataAccess2.Crc32Table();

            uint num = uint.MaxValue;
            for (int i = 0; i < bytes.Length; i++)
            {
                num = (uint) (array[(num ^ bytes[i]) & 0xFF] ^ (int) (num >> 8));
            }

            return (int) (~num);
        }

        private static int[] Crc32Table()
        {
            int[] array = new int[256];
            for (uint num = 0u; num < 256; num++)
            {
                uint num2 = num;
                for (int i = 0; i< 8; i++)
                {
                    num2 = (uint) (((num2 & 1) != 0) ? (-306674912 ^ (int) (num2 >> 1)) : ((int)(num2 >> 1)));
                }

                array[num] = (int) num2;
            }

            return array;
        }

        private static string GetHash(int crc32)
        {
            using (MD5 mD = MD5.Create())
            {
                byte[] array = mD.ComputeHash(Encoding.UTF8.GetBytes($"L{crc32}L"));

                StringBuilder stringBuilder = new();
                for (int i = 0; i < array.Length; i++)
                {
                    stringBuilder.Append(array[i].ToString("x2"));
                }

                return $"{DataAccess2.IntOrZero(stringBuilder[9])}12345{DataAccess2.IntOrZero(stringBuilder[18]) + 56 - 32}8{DataAccess2.IntOrZero(stringBuilder[31]) + 11 - 11}10A{stringBuilder[16]}CD";
            }
        }

        private static int IntOrZero(char c) => char.IsNumber(c) ? int.Parse(c.ToString()) : 0;

        private readonly ILogger<DataAccess2> logger;

        private readonly IReadOnlyDictionary<string, IProcedure> Procedures;

        public DataAccess2(ServerManager serverManager, ILogger<DataAccess2> logger)
        {
            this.logger = logger;

            this.Procedures = new Dictionary<string, IProcedure>()
            {
	            { "GetServers2", new GetServers2Procedure(serverManager) },
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
	            { "GetCampaignRun3", new GetCampaignRun3Procedure() },
	            { "GetMyStampCategorys", new GetMyStampCategoriesProcedure() },
	            { "CountMyStamps", new CountMyStampsProcedure() },
	            { "GetMyStamps", new GetMyStampsProcedure() },
	            { "SaveStamp", new SaveStampProcedure() },
	            { "GetManyStamps", new GetManyStampsProcedure() },
	            { "DeleteStamp", new DeleteStampProcedure() },
            };
        }

        [HttpPost]
        public async Task<object> DataAccessAsync([FromQuery] uint id, [FromForm] uint dataRequestID, [FromForm(Name = "gameId")] string gameIdEncoded, [FromForm(Name = "storedProcID")] string storedProcIdEncoded, [FromForm(Name = "storedProcedureName")] string storedProcedureNameEncoded, [FromForm(Name = "parametersXML")] string parametersXmlEncoded, [FromForm(Name = "platform")] string platform, [FromForm(Name = "playerType")] string playerType)
        {
            if (id == dataRequestID)
            {
                if (gameIdEncoded != null && storedProcIdEncoded != null && storedProcedureNameEncoded != null && parametersXmlEncoded != null)
                {
                    byte[] storedProcId = Convert.FromBase64String(storedProcIdEncoded);
                    byte[] key = await this.FindEncryptionKeyAsync(storedProcId, gameIdEncoded, platform, playerType);
                    if (key != null)
                    {
                        string storedProcedureName = this.DecryptData(storedProcedureNameEncoded, storedProcId, key);
                        if (this.Procedures.TryGetValue(storedProcedureName, out IProcedure procedure))
                        {
                            XDocument xml = XDocument.Parse(this.DecryptData(parametersXmlEncoded, storedProcId, key));

                            try
                            {
                                IDataAccessDataResponse response = await procedure.GetResponseAsync(this.HttpContext, xml);
                                response.DataRequestId = dataRequestID;
                                return response;
                            }
                            catch (DataAccessProcedureMissingData)
                            {
                                return new DataAccessErrorResponse(dataRequestID, "Invalid request, procedure was missing required data");
                            }
                            catch (Exception ex)
                            {
                                this.logger.LogError(EventIds.DataAccess2Failed, ex, "Failed to execute procedure");

                                return new DataAccessErrorResponse(dataRequestID, "Critical error while executing procedure");
                            }
                        }
                        else
                        {
                            return new DataAccessErrorResponse(dataRequestID, "No procedure found by the name");
                        }
                    }
                    else
                    {
                        return new DataAccessEmptyResponse(dataRequestID);
                    }
                }
            }

            return this.BadRequest();
        }

        private async Task<byte[]> FindEncryptionKeyAsync(byte[] storedProcId, string gameIdEncoded, string platform, string playerType)
        {
			ICollection<byte[]> keys = platform switch
			{
				"iOS" => DataAccess2.KEYS["iOS"].Keys,
				"AND" => DataAccess2.KEYS["Android"].Keys,

				_ => playerType switch
				{
					"ActiveX" or "PlugIn" => DataAccess2.KEYS["Browser"].Keys,
					_ => DataAccess2.KEYS["Standalone"].Keys,
				},
			};
			foreach (byte[] key in keys)
            {
                string gameId = this.DecryptData(gameIdEncoded, storedProcId, key);
                if (gameId == "f1c25e3bd3523110394b5659c68d8092")
                {
                    return key;
                }
            }

            uint userId = this.HttpContext.IsAuthenicatedPr3User();
            if (userId > 0)
            {
                PlayerUserData player = await UserManager.TryGetUserDataByIdAsync(userId, true);
                if (player != null && player.HasPermissions("debug"))
                {
                    string gameId = this.DecryptData(gameIdEncoded, storedProcId, DataAccess2.DEFAULT_KEY);
                    if (gameId == "f1c25e3bd3523110394b5659c68d8092")
                    {
                        return DataAccess2.DEFAULT_KEY;
                    }
                }
            }

            return null;
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

                using (MemoryStream memoryStream = new())
                {
                    using (ICryptoTransform decryptor = crypt.CreateDecryptor())
                    {
                        using (CryptoStream stream = new(memoryStream, decryptor, CryptoStreamMode.Write))
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