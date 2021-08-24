using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Npgsql;
using PlatformRacing3.Common.Database;
using PlatformRacing3.Common.Utils;

namespace PlatformRacing3.Common.Stamp
{
    public sealed class StampManager
    {
        private static readonly ILogger<StampManager> logger = LoggerUtil.LoggerFactory.CreateLogger<StampManager>();

        public static Task<IList<StampData>> GetStampsAsync(params uint[] stampIds)
        {
            return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT DISTINCT ON(t.id) t.id, b.version, t.author_user_id, t.title, t.category, b.description, b.art, b.last_updated FROM base.stamps_titles t JOIN base.stamps b ON b.id = t.id WHERE t.id = ANY({stampIds}) ORDER BY t.id, b.version DESC LIMIT {stampIds.Length}").ContinueWith(StampManager.ParseSqlGetStamps));
        }

        public static Task<uint> CountMyStampsAsync(uint userId, string category)
        {
            if (userId == 0)
            {
                throw new ArgumentException(null, nameof(userId));
            }

            //TODO: CACHE

            FormattableString query;
            if (category == "default-all-stamps")
            {
                query = $"SELECT COUNT(id) AS count FROM base.stamps_titles WHERE author_user_id = {userId}";
            }
            else if (category == "default-all-stamps-without-category")
            {
                query = $"SELECT COUNT(id) AS count FROM base.stamps_titles WHERE author_user_id = {userId} AND category = ''";
            }
            else if (category.StartsWith("category-"))
            {
                query = $"SELECT COUNT(id) AS count FROM base.stamps_titles WHERE author_user_id = {userId} AND category ILIKE {category["category-".Length..]}";
            }
            else
            {
                throw new ArgumentException(null, nameof(category));
            }

            return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync(query).ContinueWith(StampManager.ParseSqlReadCountMyStamps));
        }

        public static Task<ISet<string>> GetMyStampCategoriesAsync(uint userId)
        {
            if (userId == 0)
            {
                throw new ArgumentException(null, nameof(userId));
            }

            return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT DISTINCT ON (lower(category)) category FROM base.stamps_titles WHERE author_user_id = {userId} AND category != '' ORDER BY lower(category)").ContinueWith(StampManager.ParseSqlGetStampCategories));
        }

        public static Task<ISet<uint>> GetMyStampsAsync(uint userId, string category, uint start = 0, uint count = uint.MaxValue)
        {
            if (userId == 0)
            {
                throw new ArgumentException(null, nameof(userId));
            }

            FormattableString query;
            if (category == "default-all-stamps")
            {
                query = $"SELECT b.id FROM(SELECT DISTINCT ON(t.id) t.id, b.last_updated FROM base.stamps_titles t JOIN base.stamps b ON b.id = t.id WHERE t.author_user_id = {userId} ORDER BY t.id, b.last_updated DESC) AS b ORDER BY b.last_updated DESC OFFSET {start} LIMIT {count}";
            }
            else if (category == "default-all-stamps-without-category")
            {
                query = $"SELECT b.id FROM(SELECT DISTINCT ON(t.id) t.id, b.last_updated FROM base.stamps_titles t JOIN base.stamps b ON b.id = t.id WHERE t.author_user_id = {userId} AND t.category = '' ORDER BY t.id, b.last_updated DESC) AS b ORDER BY b.last_updated DESC OFFSET {start} LIMIT {count}";
            }
            else if (category.StartsWith("category-"))
            {
                query = $"SELECT b.id FROM(SELECT DISTINCT ON(t.id) t.id, b.last_updated FROM base.stamps_titles t JOIN base.stamps b ON b.id = t.id WHERE t.author_user_id = {userId} AND t.category ILIKE {category["category-".Length..]} ORDER BY t.id, b.last_updated DESC) AS b ORDER BY b.last_updated DESC OFFSET {start} LIMIT {count}";
            }
            else
            {
                throw new ArgumentException(null, nameof(category));
            }

            return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync(query).ContinueWith(StampManager.ParseSqlGetMyStamps));
        }

        public static Task<bool> SaveStampAsync(uint userId, string title, string category, string description, string art)
        {
            if (userId == 0)
            {
                throw new ArgumentException(null, nameof(userId));
            }

            return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"WITH insertTitle AS (INSERT INTO base.stamps_titles(title, category, author_user_id) VALUES({title}, {category}, {userId}) ON CONFLICT(lower(title::text), lower(category::text), author_user_id) DO UPDATE SET title = stamps_titles.title RETURNING id) INSERT INTO base.stamps(id, version, description, art) SELECT (SELECT id FROM insertTitle), COALESCE(MAX(version) + 1, 1), {description}, {art} FROM base.stamps WHERE id = (SELECT id FROM insertTitle) RETURNING id").ContinueWith(StampManager.ParseSqlSaveStamp));
        }

        public static Task<bool> DeleteStampAsync(uint stampId, uint authorId = default)
        {
            if (authorId != default)
            {
                return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"WITH deleted AS (DELETE FROM base.stamps_titles WHERE id = {stampId} AND author_user_id = {authorId} RETURNING id, title, category, author_user_id) INSERT INTO base.stamps_deleted(id, title, category, author_user_id) SELECT id, title, category, author_user_id FROM deleted RETURNING ID").ContinueWith(StampManager.ParseSqlDeleteStamp));
            }
            else
            {
                return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"WITH deleted AS (DELETE FROM base.stamps_titles WHERE id = {stampId} AND author_user_id = {authorId} RETURNING id, title, category, author_user_id) INSERT INTO base.stamps_deleted(id, title, category, author_user_id) SELECT id, title, category, author_user_id FROM deleted RETURNING ID").ContinueWith(StampManager.ParseSqlDeleteStamp));
            }
        }

        private static IList<StampData> ParseSqlGetStamps(Task<NpgsqlDataReader> task)
        {
            IList<StampData> stamps = new List<StampData>();
            if (task.IsCompletedSuccessfully)
            {
                DbDataReader reader = task.Result;
                while (reader?.Read() ?? false)
                {
                    stamps.Add(new StampData(reader));
                }
            }
            else if (task.IsFaulted)
            {
                StampManager.logger.LogError(EventIds.StampLoadFailed, task.Exception, $"Failed to load {nameof(StampData)} from sql");
            }

            return stamps;
        }

        private static uint ParseSqlReadCountMyStamps(Task<NpgsqlDataReader> task)
        {
            if (task.IsCompletedSuccessfully)
            {
                DbDataReader reader = task.Result;
                if (reader?.Read() ?? false)
                {
                    return (uint)(long)reader["count"];
                }
            }
            else if (task.IsFaulted)
            {
	            StampManager.logger.LogError(EventIds.StampLoadFailed, task.Exception, "Failed to count users stamp count from sql");
            }

            return 0u;
        }

        private static ISet<string> ParseSqlGetStampCategories(Task<NpgsqlDataReader> task)
        {
            ISet<string> categories = new HashSet<string>();
            if (task.IsCompletedSuccessfully)
            {
                DbDataReader reader = task.Result;
                while (reader?.Read() ?? false)
                {
                    categories.Add((string)reader["category"]);
                }
            }
            else if (task.IsFaulted)
            {
	            StampManager.logger.LogError(EventIds.StampLoadFailed, task.Exception, "Failed to load users stamp categories from sql");
            }

            return categories;
        }

        private static ISet<uint> ParseSqlGetMyStamps(Task<NpgsqlDataReader> task)
        {
            HashSet<uint> stamps = new();
            if (task.IsCompletedSuccessfully)
            {
                DbDataReader reader = task.Result;
                while (reader?.Read() ?? false)
                {
                    stamps.Add((uint)(int)reader["id"]);
                }
            }
            else if (task.IsFaulted)
            {
	            StampManager.logger.LogError(EventIds.StampLoadFailed, task.Exception, "Failed to load user stamps from sql");
            }

            return stamps;
        }

        private static bool ParseSqlSaveStamp(Task<NpgsqlDataReader> task)
        {
            if (task.IsCompletedSuccessfully)
            {
                DbDataReader reader = task.Result;
                if (reader?.Read() ?? false)
                {
                    uint stampId = (uint)(int)reader["id"];

                    return stampId > 0;
                }
            }
            else if (task.IsFaulted)
            {
	            StampManager.logger.LogError(EventIds.StampSaveFailed, task.Exception, "Failed to save stamp to sql");
            }

            return false;
        }

        private static bool ParseSqlDeleteStamp(Task<NpgsqlDataReader> task)
        {
            if (task.IsCompletedSuccessfully)
            {
                DbDataReader reader = task.Result;
                if (reader?.Read() ?? false)
                {
                    uint stampId = (uint)(int)reader["id"];

                    return stampId > 0;
                }
            }
            else if (task.IsFaulted)
            {
	            StampManager.logger.LogError(EventIds.StampSaveFailed, task.Exception, "Failed to delete stamp from sql");
            }

            return false;
        }
    }
}
