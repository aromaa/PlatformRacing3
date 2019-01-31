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
        internal static List<CampaignPrize> DefaultPrizes { get; private set; } = new List<CampaignPrize>();
        internal static Dictionary<uint, Dictionary<CampaignMedal, uint>> DefaultCampaignTimes { get; private set; } = new Dictionary<uint, Dictionary<CampaignMedal, uint>>();

        private Dictionary<uint, Dictionary<CampaignMedal, uint>> _CampaignTimes;
        private List<CampaignPrize> _Prizes;

        public CampaignManager()
        {
            this._CampaignTimes = new Dictionary<uint, Dictionary<CampaignMedal, uint>>();
            this._Prizes = new List<CampaignPrize>();
        }

        public async Task LoadCampaignTimesAsync()
        {
            Dictionary<uint, Dictionary<CampaignMedal, uint>>  times = new Dictionary<uint, Dictionary<CampaignMedal, uint>>();
            using (DatabaseConnection dbConnection = new DatabaseConnection())
            {
                DbDataReader reader = await dbConnection.ReadDataAsync($"SELECT level_id, bronze_time, silver_time, gold_time FROM base.campaigns");
                while (reader?.Read() ?? false)
                {
                    Dictionary<CampaignMedal, uint> level = new Dictionary<CampaignMedal, uint>()
                    {
                        { CampaignMedal.Bronze, (uint)(int)reader["bronze_time"] * 1000 },
                        { CampaignMedal.Silver, (uint)(int)reader["silver_time"] * 1000 },
                        { CampaignMedal.Gold, (uint)(int)reader["gold_time"] * 1000 },
                    };

                    times.Add((uint)(int)reader["level_id"], level);
                }
            }

            CampaignManager.DefaultCampaignTimes = times;
            this._CampaignTimes = times; //Thread-safety
        }

        public async Task LoadPrizesAsync()
        {
            List<CampaignPrize> prizes = new List<CampaignPrize>();
            using (DatabaseConnection dbConnection = new DatabaseConnection())
            {
                DbDataReader reader = await dbConnection.ReadDataAsync($"SELECT id, type, medals_required FROM base.campaigns_prizes ORDER BY medals_required");
                while (reader?.Read() ?? false)
                {
                    prizes.Add(new CampaignPrize(reader));
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

        public IReadOnlyList<CampaignPrize> Prizes => this._Prizes.AsReadOnly();
    }
}
