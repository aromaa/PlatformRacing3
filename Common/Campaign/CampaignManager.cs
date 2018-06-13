using Platform_Racing_3_Common.Database;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Platform_Racing_3_Common.Campaign
{
    public class CampaignManager
    {
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

        public static Task SaveCampaignRun(uint userId, uint levelId, uint levelVersion, string recordedRun, uint finishTime)
        {
            return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ExecuteNonQueryAsync($"INSERT INTO base.campaigns_runs(level_id, level_version, user_id, recorded_run, finish_time) VALUES({levelId}, {levelVersion}, {userId}, {recordedRun}, {finishTime})"));
        }

        public IReadOnlyList<CampaignPrize> Prizes => this._Prizes.AsReadOnly();
    }
}
