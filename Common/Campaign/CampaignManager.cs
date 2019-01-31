using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using log4net;
using Newtonsoft.Json;
using Platform_Racing_3_Common.Database;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Platform_Racing_3_Common.Campaign
{
    public class CampaignManager
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Jiggmin
        /// </summary>
        internal static Dictionary<string, List<CampaignPrize>> DefaultPrizes { get; private set; } = new Dictionary<string, List<CampaignPrize>>();
        internal static Dictionary<uint, (string Season, Dictionary<CampaignMedal, uint> Medals)> DefaultCampaignTimes { get; private set; } = new Dictionary<uint, (string, Dictionary<CampaignMedal, uint>)>();

        private Dictionary<uint, (string Season, Dictionary<CampaignMedal, uint> Medals)> _CampaignTimes;
        private Dictionary<string, List<CampaignPrize>> _Prizes;

        public CampaignManager()
        {
            this._CampaignTimes = new Dictionary<uint, (string, Dictionary<CampaignMedal, uint>)>();
            this._Prizes = new Dictionary<string, List<CampaignPrize>>();
        }

        public async Task LoadCampaignTimesAsync()
        {
            Dictionary<uint, (string Season, Dictionary<CampaignMedal, uint> Medals)>  times = new Dictionary<uint, (string, Dictionary<CampaignMedal, uint>)>();
            using (DatabaseConnection dbConnection = new DatabaseConnection())
            {
                DbDataReader reader = await dbConnection.ReadDataAsync($"SELECT level_id, bronze_time, silver_time, gold_time, season FROM base.campaigns");
                while (reader?.Read() ?? false)
                {
                    Dictionary<CampaignMedal, uint> level = new Dictionary<CampaignMedal, uint>()
                    {
                        { CampaignMedal.Bronze, (uint)(int)reader["bronze_time"] * 1000 },
                        { CampaignMedal.Silver, (uint)(int)reader["silver_time"] * 1000 },
                        { CampaignMedal.Gold, (uint)(int)reader["gold_time"] * 1000 },
                    };

                    times.Add((uint)(int)reader["level_id"], ((string)reader["season"], level));
                }
            }

            CampaignManager.DefaultCampaignTimes = times;
            this._CampaignTimes = times; //Thread-safety
        }

        public async Task LoadPrizesAsync()
        {
            Dictionary<string, List<CampaignPrize>> prizes = new Dictionary<string, List<CampaignPrize>>();
            using (DatabaseConnection dbConnection = new DatabaseConnection())
            {
                DbDataReader reader = await dbConnection.ReadDataAsync($"SELECT id, type, medals_required, season FROM base.campaigns_prizes ORDER BY medals_required");
                while (reader?.Read() ?? false)
                {
                    string season = (string)reader["season"];
                    if (!prizes.TryGetValue(season, out List<CampaignPrize> prizeList))
                    {
                        prizes[season] = prizeList = new List<CampaignPrize>();
                    }

                    prizeList.Add(new CampaignPrize(reader));
                }
            }

            CampaignManager.DefaultPrizes = prizes;
            this._Prizes = prizes; //Thread-safety
        }

        public static Task SaveCampaignRunAsync(uint userId, uint levelId, uint levelVersion, string recordedRun, int finishTime)
        {
            return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ExecuteNonQueryAsync($"INSERT INTO base.campaigns_runs(level_id, level_version, user_id, recorded_run, finish_time) VALUES({levelId}, {levelVersion}, {userId}, {recordedRun}, {finishTime})"));
        }

        //Change stuff for this
        public static Task<IReadOnlyDictionary<uint, (int Time, CampaignRun Run)>> GetFriendRunsAsync(uint userId, uint levelId)
        {
            return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT * FROM(SELECT DISTINCT ON (r.user_id) r.user_id, r.finish_time, r.recorded_run FROM base.friends f RIGHT JOIN base.campaigns_runs r ON r.user_id = f.friend_user_id WHERE f.user_id = {userId} AND r.level_id = {levelId}) AS r ORDER BY SIGN(r.finish_time) DESC, r.finish_time ASC LIMIT 3").ContinueWith(CampaignManager.ParseSqlFriendsRuns));
        }
        public static Task<string> GetRawRunAsync(uint levelId, uint userId)
        {
            return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT recorded_run FROM base.campaigns_runs WHERE user_id = {userId} AND level_id = {levelId} ORDER BY SIGN(finish_time) DESC, finish_time ASC LIMIT 1").ContinueWith(CampaignManager.ParseSqlRawRun));
        }

        private static IReadOnlyDictionary<uint, (int Time, CampaignRun Run)> ParseSqlFriendsRuns(Task<DbDataReader> task)
        {
            if (task.IsCompletedSuccessfully)
            {
                IDictionary<uint, (int Time, CampaignRun Run)> runs = new Dictionary<uint, (int, CampaignRun)>(3);

                DbDataReader reader = task.Result;
                while (reader?.Read() ?? false)
                {
                    CampaignRun campaignRun;
                    using (MemoryStream compressedMemoryStream = new MemoryStream(Convert.FromBase64String((string)reader["recorded_run"])))
                    {
                        using (InflaterInputStream inflater = new InflaterInputStream(compressedMemoryStream))
                        {
                            using (MemoryStream uncompressedMemoryStream = new MemoryStream())
                            {
                                inflater.CopyTo(uncompressedMemoryStream);

                                campaignRun = JsonConvert.DeserializeObject<CampaignRun>(Encoding.UTF8.GetString(uncompressedMemoryStream.ToArray()));
                            }
                        }
                    }

                    runs.Add((uint)(int)reader["user_id"], ((int)reader["finish_time"], campaignRun));
                }

                return (IReadOnlyDictionary<uint, (int, CampaignRun)>)runs;
            }
            else if (task.IsFaulted)
            {
                CampaignManager.Logger.Error($"Failed to load friends runs from sql", task.Exception);
            }

            return null;
        }
        private static string ParseSqlRawRun(Task<DbDataReader> task)
        {
            if (task.IsCompletedSuccessfully)
            {
                DbDataReader reader = task.Result;
                while (reader?.Read() ?? false)
                {
                    return (string)reader["recorded_run"];
                }
            }
            else if (task.IsFaulted)
            {
                CampaignManager.Logger.Error($"Failed to load raw run from sql", task.Exception);
            }

            return null;
        }

        public IReadOnlyDictionary<string, List<CampaignPrize>> Prizes => (IReadOnlyDictionary<string, List<CampaignPrize>>)this._Prizes;
    }
}
