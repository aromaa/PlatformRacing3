using log4net;
using Platform_Racing_3_Common.Database;
using Platform_Racing_3_Common.User;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Platform_Racing_3_Common.Level
{
    public class LevelManager
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        //TODO: Implement caching, this is not done here yet due to the pure complexity, it should be done at once and not left half way done (Like user caching, lol)

        public static Task<LevelData> GetLevelDataAsync(uint levelId)
        {
            if (levelId == 0)
            {
                throw new ArgumentException(nameof(levelId));
            }

            //TODO: Cache internally and listen for redis update event?

            return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT DISTINCT ON(t.id) t.id, l.version, t.author_user_id, COALESCE(u.username, 'Unknown') AS author_username, COALESCE(u.name_color, -14855017) AS author_name_color, t.title, l.description, l.publish, l.song_id, l.mode, l.seconds, l.gravity, l.alien, l.sfchm, l.snow, l.wind, l.items, l.health, l.king_of_the_hat, l.bg_image, l.level_data, l.last_updated, COALESCE(p.plays, 0) AS plays, COUNT(t.id) FILTER(WHERE r.rating = 'like') OVER(PARTITION BY t.id, l.version) AS likes, COUNT(t.id) FILTER(WHERE r.rating = 'dislike') OVER(PARTITION BY t.id, l.version) AS dislikes, CASE WHEN c.level_id IS NULL THEN false ELSE true END AS is_campaign, c.bronze_time, c.silver_time, c.gold_time, c.medals_required, c.season AS campaign_season, CASE WHEN z.level_id IS NULL THEN false ELSE true END AS has_prize, array_agg(ARRAY[z.part_type::text, z.part_id::text]) OVER(PARTITION BY t.id, l.version) AS prizes FROM base.levels_titles t JOIN base.levels l ON l.id = t.id LEFT JOIN base.users u ON u.id = t.author_user_id LEFT JOIN base.levels_plays p ON p.level_id = l.id LEFT JOIN base.levels_ratings r ON r.level_id = l.id LEFT JOIN base.campaigns c ON c.level_id = t.id LEFT JOIN base.levels_prize z ON z.level_id = t.id WHERE l.id = {levelId} ORDER BY t.id, l.version DESC").ContinueWith(LevelManager.ParseSqlLevelData));
        }

        public static Task<LevelData> GetLevelDataAsync(uint userId, string title)
        {
            return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT DISTINCT ON(t.id) t.id, l.version, t.author_user_id, COALESCE(u.username, 'Unknown') AS author_username, COALESCE(u.name_color, -14855017) AS author_name_color, t.title, l.description, l.publish, l.song_id, l.mode, l.seconds, l.gravity, l.alien, l.sfchm, l.snow, l.wind, l.items, l.health, l.king_of_the_hat, l.bg_image, l.level_data, l.last_updated, COALESCE(p.plays, 0) AS plays, COUNT(t.id) FILTER(WHERE r.rating = 'like') OVER(PARTITION BY t.id, l.version) AS likes, COUNT(t.id) FILTER(WHERE r.rating = 'dislike') OVER(PARTITION BY t.id, l.version) AS dislikes, CASE WHEN c.level_id IS NULL THEN false ELSE true END AS is_campaign, c.bronze_time, c.silver_time, c.gold_time, c.medals_required, c.season AS campaign_season, CASE WHEN z.level_id IS NULL THEN false ELSE true END AS has_prize, array_agg(ARRAY[z.part_type::text, z.part_id::text]) OVER(PARTITION BY t.id, l.version) AS prizes FROM base.levels_titles t JOIN base.levels l ON l.id = t.id LEFT JOIN base.users u ON u.id = t.author_user_id LEFT JOIN base.levels_plays p ON p.level_id = l.id LEFT JOIN base.levels_ratings r ON r.level_id = l.id LEFT JOIN base.campaigns c ON c.level_id = t.id LEFT JOIN base.levels_prize z ON z.level_id = t.id WHERE t.author_user_id = {userId} AND t.title ILIKE {title} ORDER BY t.id, l.version DESC").ContinueWith(LevelManager.ParseSqlLevelData));
        }

        public static Task<LevelData> GetLevelDataAsync(uint levelId, uint version)
        {
            if (levelId == 0)
            {
                throw new ArgumentException(nameof(levelId));
            }

            if (version == 0)
            {
                throw new ArgumentException(nameof(levelId));
            }

            //TODO: Cache internally and listen for redis update event?

            return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT t.id, l.version, t.author_user_id, COALESCE(u.username, 'Unknown') AS author_username, COALESCE(u.name_color, -14855017) AS author_name_color, t.title, l.description, l.publish, l.song_id, l.mode, l.seconds, l.gravity, l.alien, l.sfchm, l.snow, l.wind, l.items, l.health, l.king_of_the_hat, l.bg_image, l.level_data, l.last_updated, COALESCE(p.plays, 0) AS plays, COUNT(t.id) FILTER(WHERE r.rating = 'like') OVER(PARTITION BY t.id, l.version) AS likes, COUNT(t.id) FILTER(WHERE r.rating = 'dislike') OVER(PARTITION BY t.id, l.version) AS dislikes, CASE WHEN c.level_id IS NULL THEN false ELSE true END AS is_campaign, c.bronze_time, c.silver_time, c.gold_time, c.medals_required, c.season AS campaign_season, CASE WHEN z.level_id IS NULL THEN false ELSE true END AS has_prize, array_agg(ARRAY[z.part_type::text, z.part_id::text]) OVER(PARTITION BY t.id, l.version) AS prizes FROM base.levels_titles t JOIN base.levels l ON l.id = t.id LEFT JOIN base.users u ON u.id = t.author_user_id LEFT JOIN base.levels_plays p ON p.level_id = l.id LEFT JOIN base.levels_ratings r ON r.level_id = l.id LEFT JOIN base.campaigns c ON c.level_id = t.id LEFT JOIN base.levels_prize z ON z.level_id = t.id WHERE l.id = {levelId} AND l.version = {version} ORDER BY t.id DESC LIMIT 1").ContinueWith(LevelManager.ParseSqlLevelData));
        }

        public static Task<uint> CountMyLevelsAsync(uint userId)
        {
            if (userId == 0)
            {
                throw new ArgumentException(nameof(userId));
            }

            //TODO: Cache

            return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT COUNT(id) AS count FROM base.levels_titles WHERE author_user_id = {userId}").ContinueWith(LevelManager.ParseSqlCountMyLevels));
        }

        public static Task<IReadOnlyCollection<LevelData>> GetMyLevelsAsync(uint authorId, uint start = 0, uint count = uint.MaxValue)
        {
            if (authorId == 0)
            {
                throw new ArgumentException(nameof(authorId));
            }

            //TODO: Cache

            return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT * FROM(SELECT DISTINCT ON(t.id) t.id, l.version, t.author_user_id, COALESCE(u.username, 'Unknown') AS author_username, COALESCE(u.name_color, -14855017) AS author_name_color, t.title, l.description, l.publish, l.song_id, l.mode, l.seconds, l.gravity, l.alien, l.sfchm, l.snow, l.wind, l.items, l.health, l.king_of_the_hat, l.bg_image, l.level_data, l.last_updated, COALESCE(p.plays, 0) AS plays, COUNT(t.id) FILTER(WHERE r.rating = 'like') OVER(PARTITION BY t.id, l.version) AS likes, COUNT(t.id) FILTER(WHERE r.rating = 'dislike') OVER(PARTITION BY t.id, l.version) AS dislikes, CASE WHEN c.level_id IS NULL THEN false ELSE true END AS is_campaign, c.bronze_time, c.silver_time, c.gold_time, c.medals_required, c.season AS campaign_season, CASE WHEN z.level_id IS NULL THEN false ELSE true END AS has_prize, array_agg(ARRAY[z.part_type::text, z.part_id::text]) OVER(PARTITION BY t.id, l.version) AS prizes FROM base.levels_titles t JOIN base.levels l ON l.id = t.id LEFT JOIN base.users u ON u.id = t.author_user_id LEFT JOIN base.levels_plays p ON p.level_id = l.id LEFT JOIN base.levels_ratings r ON r.level_id = l.id LEFT JOIN base.campaigns c ON c.level_id = t.id LEFT JOIN base.levels_prize z ON z.level_id = t.id WHERE t.author_user_id = {authorId} ORDER BY t.id, l.version DESC) AS l ORDER BY l.last_updated DESC OFFSET {start} LIMIT {count}").ContinueWith(LevelManager.ParseSqlLevelDataMultiple));
        }

        public static Task<uint> SaveLevelAsync(uint userId, string title, string description, bool publish, string songId, LevelMode mode, uint seconds, double gravity, float alien, float sfchm, float snow, float wind, string[] items, uint health, int[] kingOfTheHat, string bgImage, string levelData)
        {
            if (userId == 0)
            {
                throw new ArgumentException(nameof(userId));
            }

            return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"WITH insertTitle AS (INSERT INTO base.levels_titles(title, author_user_id) VALUES({title}, {userId}) ON CONFLICT(lower(title::text), author_user_id) DO UPDATE SET title = levels_titles.title RETURNING id) INSERT INTO base.levels(id, version, description, publish, song_id, mode, seconds, gravity, alien, sfchm, snow, wind, items, health, king_of_the_hat, bg_image, level_data) SELECT (SELECT id FROM insertTitle) AS id, COALESCE(MAX(version) + 1, 1), {description}, {publish}, {songId}, {mode}, {seconds}, {gravity}, {alien}, {sfchm}, {snow}, {wind}, {items}, {health}, {kingOfTheHat}, {bgImage}, {levelData} FROM base.levels WHERE id = (SELECT id FROM insertTitle) RETURNING id").ContinueWith(LevelManager.ParseSqlSaveLevel));
        }

        public static Task<bool> DeleteLevelAsync(uint levelId, uint? authorId = default)
        {
            if (levelId == 0)
            {
                throw new ArgumentException(nameof(levelId));
            }

            if (authorId != default)
            {
                return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"WITH deleted AS (DELETE FROM base.levels_titles WHERE id = {levelId} AND author_user_id = {authorId} RETURNING id, title, author_user_id) INSERT INTO base.levels_deleted(id, title, author_user_id) SELECT id, title, author_user_id FROM deleted RETURNING ID").ContinueWith(LevelManager.ParseSqlDeleteLevel));
            }
            else
            {
                return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"WITH deleted AS (DELETE FROM base.levels_titles WHERE id = {levelId} RETURNING id, title, author_user_id) INSERT INTO base.levels_deleted(id, title, author_user_id) SELECT id, title, author_user_id FROM deleted RETURNING ID").ContinueWith(LevelManager.ParseSqlDeleteLevel));
            }
        }

        public static Task<(uint results, IReadOnlyCollection<LevelData>)> GetNewestLevels(uint start = 0, uint count = uint.MaxValue, UserData user = null)
        {
            if (!user?.IsGuest ?? false)
            {
                if (user.HasPermissions(Permissions.ACCESS_SEE_UNPUBLISHED_LEVELS))
                {
                    return DatabaseConnection.NewAsyncConnection((dbconnection) => dbconnection.ReadDataAsync($"SELECT COUNT(l.id) OVER() AS results, * FROM(SELECT DISTINCT ON(t.id) t.id, l.version, t.author_user_id, COALESCE(u.username, 'Unknown') AS author_username, COALESCE(u.name_color, -14855017) AS author_name_color, t.title, l.description, l.publish, l.song_id, l.mode, l.seconds, l.gravity, l.alien, l.sfchm, l.snow, l.wind, l.items, l.health, l.king_of_the_hat, l.bg_image, l.level_data, l.last_updated, COALESCE(p.plays, 0) AS plays, COUNT(t.id) FILTER(WHERE r.rating = 'like') OVER(PARTITION BY t.id, l.version) AS likes, COUNT(t.id) FILTER(WHERE r.rating = 'dislike') OVER(PARTITION BY t.id, l.version) AS dislikes, CASE WHEN c.level_id IS NULL THEN false ELSE true END AS is_campaign, c.bronze_time, c.silver_time, c.gold_time, c.medals_required, c.season AS campaign_season, CASE WHEN z.level_id IS NULL THEN false ELSE true END AS has_prize, array_agg(ARRAY[z.part_type::text, z.part_id::text]) OVER(PARTITION BY t.id, l.version) AS prizes FROM base.levels_titles t JOIN base.levels l ON l.id = t.id LEFT JOIN base.users u ON u.id = t.author_user_id LEFT JOIN base.levels_plays p ON p.level_id = l.id LEFT JOIN base.levels_ratings r ON r.level_id = l.id LEFT JOIN base.campaigns c ON c.level_id = t.id LEFT JOIN base.levels_prize z ON z.level_id = t.id ORDER BY t.id, l.version DESC) AS l ORDER BY l.last_updated DESC OFFSET {start} LIMIT {count}").ContinueWith(LevelManager.ParseSqlLevelDataMultipleAndTotal));
                }
                else
                {
                    return DatabaseConnection.NewAsyncConnection((dbconnection) => dbconnection.ReadDataAsync($"SELECT COUNT(l.id) OVER() AS results, * FROM(SELECT DISTINCT ON(t.id) t.id, l.version, t.author_user_id, COALESCE(u.username, 'Unknown') AS author_username, COALESCE(u.name_color, -14855017) AS author_name_color, t.title, l.description, l.publish, l.song_id, l.mode, l.seconds, l.gravity, l.alien, l.sfchm, l.snow, l.wind, l.items, l.health, l.king_of_the_hat, l.bg_image, l.level_data, l.last_updated, COALESCE(p.plays, 0) AS plays, COUNT(t.id) FILTER(WHERE r.rating = 'like') OVER(PARTITION BY t.id, l.version) AS likes, COUNT(t.id) FILTER(WHERE r.rating = 'dislike') OVER(PARTITION BY t.id, l.version) AS dislikes, CASE WHEN c.level_id IS NULL THEN false ELSE true END AS is_campaign, c.bronze_time, c.silver_time, c.gold_time, c.medals_required, c.season AS campaign_season, CASE WHEN z.level_id IS NULL THEN false ELSE true END AS has_prize, array_agg(ARRAY[z.part_type::text, z.part_id::text]) OVER(PARTITION BY t.id, l.version) AS prizes FROM base.levels_titles t JOIN base.levels l ON l.id = t.id LEFT JOIN base.users u ON u.id = t.author_user_id LEFT JOIN base.levels_plays p ON p.level_id = l.id LEFT JOIN base.levels_ratings r ON r.level_id = l.id LEFT JOIN base.campaigns c ON c.level_id = t.id LEFT JOIN base.levels_prize z ON z.level_id = t.id WHERE t.author_user_id = {user.Id} OR l.publish = '1' ORDER BY t.id, l.version DESC) AS l ORDER BY l.last_updated DESC OFFSET {start} LIMIT {count}").ContinueWith(LevelManager.ParseSqlLevelDataMultipleAndTotal));
                }
            }

            return DatabaseConnection.NewAsyncConnection((dbconnection) => dbconnection.ReadDataAsync($"SELECT COUNT(l.id) OVER() AS results, * FROM(SELECT DISTINCT ON(t.id) t.id, l.version, t.author_user_id, COALESCE(u.username, 'Unknown') AS author_username, COALESCE(u.name_color, -14855017) AS author_name_color, t.title, l.description, l.publish, l.song_id, l.mode, l.seconds, l.gravity, l.alien, l.sfchm, l.snow, l.wind, l.items, l.health, l.king_of_the_hat, l.bg_image, l.level_data, l.last_updated, COALESCE(p.plays, 0) AS plays, COUNT(t.id) FILTER(WHERE r.rating = 'like') OVER(PARTITION BY t.id, l.version) AS likes, COUNT(t.id) FILTER(WHERE r.rating = 'dislike') OVER(PARTITION BY t.id, l.version) AS dislikes, CASE WHEN c.level_id IS NULL THEN false ELSE true END AS is_campaign, c.bronze_time, c.silver_time, c.gold_time, c.medals_required, c.season AS campaign_season, CASE WHEN z.level_id IS NULL THEN false ELSE true END AS has_prize, array_agg(ARRAY[z.part_type::text, z.part_id::text]) OVER(PARTITION BY t.id, l.version) AS prizes FROM base.levels_titles t JOIN base.levels l ON l.id = t.id LEFT JOIN base.users u ON u.id = t.author_user_id LEFT JOIN base.levels_plays p ON p.level_id = l.id LEFT JOIN base.levels_ratings r ON r.level_id = l.id LEFT JOIN base.campaigns c ON c.level_id = t.id LEFT JOIN base.levels_prize z ON z.level_id = t.id WHERE l.publish = '1' ORDER BY t.id, l.version DESC) AS l ORDER BY l.last_updated DESC OFFSET {start} LIMIT {count}").ContinueWith(LevelManager.ParseSqlLevelDataMultipleAndTotal));
        }

        public static Task<(uint results, IReadOnlyCollection<LevelData>)> GetBestLevels(uint start = 0, uint count = uint.MaxValue, UserData user = null)
        {
            if (!user?.IsGuest ?? false)
            {
                if (user.HasPermissions(Permissions.ACCESS_SEE_UNPUBLISHED_LEVELS))
                {
                    return DatabaseConnection.NewAsyncConnection((dbconnection) => dbconnection.ReadDataAsync($"SELECT COUNT(l.id) OVER() AS results, * FROM(SELECT DISTINCT ON(t.id) t.id, l.version, t.author_user_id, COALESCE(u.username, 'Unknown') AS author_username, COALESCE(u.name_color, -14855017) AS author_name_color, t.title, l.description, l.publish, l.song_id, l.mode, l.seconds, l.gravity, l.alien, l.sfchm, l.snow, l.wind, l.items, l.health, l.king_of_the_hat, l.bg_image, l.level_data, l.last_updated, COALESCE(p.plays, 0) AS plays, COUNT(t.id) FILTER(WHERE r.rating = 'like') OVER(PARTITION BY t.id, l.version) AS likes, COUNT(t.id) FILTER(WHERE r.rating = 'dislike') OVER(PARTITION BY t.id, l.version) AS dislikes, CASE WHEN c.level_id IS NULL THEN false ELSE true END AS is_campaign, c.bronze_time, c.silver_time, c.gold_time, c.medals_required, c.season AS campaign_season, CASE WHEN z.level_id IS NULL THEN false ELSE true END AS has_prize, array_agg(ARRAY[z.part_type::text, z.part_id::text]) OVER(PARTITION BY t.id, l.version) AS prizes FROM base.levels_titles t JOIN base.levels l ON l.id = t.id LEFT JOIN base.users u ON u.id = t.author_user_id LEFT JOIN base.levels_plays p ON p.level_id = l.id LEFT JOIN base.levels_ratings r ON r.level_id = l.id LEFT JOIN base.campaigns c ON c.level_id = t.id LEFT JOIN base.levels_prize z ON z.level_id = t.id ORDER BY t.id, l.version DESC) AS l ORDER BY CASE WHEN l.dislikes = 0 THEN l.likes WHEN l.likes = 0 THEN -l.dislikes ELSE l.likes END DESC OFFSET {start} LIMIT {count}").ContinueWith(LevelManager.ParseSqlLevelDataMultipleAndTotal));
                }
                else
                {
                    return DatabaseConnection.NewAsyncConnection((dbconnection) => dbconnection.ReadDataAsync($"SELECT COUNT(l.id) OVER() AS results, * FROM(SELECT DISTINCT ON(t.id) t.id, l.version, t.author_user_id, COALESCE(u.username, 'Unknown') AS author_username, COALESCE(u.name_color, -14855017) AS author_name_color, t.title, l.description, l.publish, l.song_id, l.mode, l.seconds, l.gravity, l.alien, l.sfchm, l.snow, l.wind, l.items, l.health, l.king_of_the_hat, l.bg_image, l.level_data, l.last_updated, COALESCE(p.plays, 0) AS plays, COUNT(t.id) FILTER(WHERE r.rating = 'like') OVER(PARTITION BY t.id, l.version) AS likes, COUNT(t.id) FILTER(WHERE r.rating = 'dislike') OVER(PARTITION BY t.id, l.version) AS dislikes, CASE WHEN c.level_id IS NULL THEN false ELSE true END AS is_campaign, c.bronze_time, c.silver_time, c.gold_time, c.medals_required, c.season AS campaign_season, CASE WHEN z.level_id IS NULL THEN false ELSE true END AS has_prize, array_agg(ARRAY[z.part_type::text, z.part_id::text]) OVER(PARTITION BY t.id, l.version) AS prizes FROM base.levels_titles t JOIN base.levels l ON l.id = t.id LEFT JOIN base.users u ON u.id = t.author_user_id LEFT JOIN base.levels_plays p ON p.level_id = l.id LEFT JOIN base.levels_ratings r ON r.level_id = l.id LEFT JOIN base.campaigns c ON c.level_id = t.id LEFT JOIN base.levels_prize z ON z.level_id = t.id WHERE t.author_user_id = {user.Id} OR l.publish = '1' ORDER BY t.id, l.version DESC) AS l ORDER BY CASE WHEN l.dislikes = 0 THEN l.likes WHEN l.likes = 0 THEN -l.dislikes ELSE l.likes END DESC OFFSET {start} LIMIT {count}").ContinueWith(LevelManager.ParseSqlLevelDataMultipleAndTotal));
                }
            }

            return DatabaseConnection.NewAsyncConnection((dbconnection) => dbconnection.ReadDataAsync($"SELECT COUNT(l.id) OVER() AS results, * FROM(SELECT DISTINCT ON(t.id) t.id, l.version, t.author_user_id, COALESCE(u.username, 'Unknown') AS author_username, COALESCE(u.name_color, -14855017) AS author_name_color, t.title, l.description, l.publish, l.song_id, l.mode, l.seconds, l.gravity, l.alien, l.sfchm, l.snow, l.wind, l.items, l.health, l.king_of_the_hat, l.bg_image, l.level_data, l.last_updated, COALESCE(p.plays, 0) AS plays, COUNT(t.id) FILTER(WHERE r.rating = 'like') OVER(PARTITION BY t.id, l.version) AS likes, COUNT(t.id) FILTER(WHERE r.rating = 'dislike') OVER(PARTITION BY t.id, l.version) AS dislikes, CASE WHEN c.level_id IS NULL THEN false ELSE true END AS is_campaign, c.bronze_time, c.silver_time, c.gold_time, c.medals_required, c.season AS campaign_season, CASE WHEN z.level_id IS NULL THEN false ELSE true END AS has_prize, array_agg(ARRAY[z.part_type::text, z.part_id::text]) OVER(PARTITION BY t.id, l.version) AS prizes FROM base.levels_titles t JOIN base.levels l ON l.id = t.id LEFT JOIN base.users u ON u.id = t.author_user_id LEFT JOIN base.levels_plays p ON p.level_id = l.id LEFT JOIN base.levels_ratings r ON r.level_id = l.id LEFT JOIN base.campaigns c ON c.level_id = t.id LEFT JOIN base.levels_prize z ON z.level_id = t.id WHERE l.publish = '1' ORDER BY t.id, l.version DESC) AS l ORDER BY CASE WHEN l.dislikes = 0 THEN l.likes WHEN l.likes = 0 THEN -l.dislikes ELSE l.likes END DESC OFFSET {start} LIMIT {count}").ContinueWith(LevelManager.ParseSqlLevelDataMultipleAndTotal));
        }

        public static Task<(uint results, IReadOnlyCollection<LevelData> Levels)> GetCampaignLevels(string season = null, uint start = 0, uint count = uint.MaxValue)
        {
            if (season == null)
            {
                return DatabaseConnection.NewAsyncConnection((dbconnection) => dbconnection.ReadDataAsync($"SELECT COUNT(l.id) OVER() AS results, * FROM(SELECT DISTINCT ON(c.level_id) c.level_id AS id, t.title, l.version, t.author_user_id, COALESCE(u.username, 'Unknown') AS author_username, COALESCE(u.name_color, -14855017) AS author_name_color, t.title, l.description, l.publish, l.song_id, l.mode, l.seconds, l.gravity, l.alien, l.sfchm, l.snow, l.wind, l.items, l.health, l.king_of_the_hat, l.bg_image, l.level_data, l.last_updated, COALESCE(p.plays, 0) AS plays, COUNT(t.id) FILTER(WHERE r.rating = 'like') OVER(PARTITION BY t.id, l.version) AS likes, COUNT(t.id) FILTER(WHERE r.rating = 'dislike') OVER(PARTITION BY t.id, l.version) AS dislikes, c.level_order, CASE WHEN c.level_id IS NULL THEN false ELSE true END AS is_campaign, c.bronze_time, c.silver_time, c.gold_time, c.medals_required, c.season AS campaign_season, CASE WHEN z.level_id IS NULL THEN false ELSE true END AS has_prize, array_agg(ARRAY[z.part_type::text, z.part_id::text]) OVER(PARTITION BY t.id, l.version) AS prizes FROM base.campaigns c JOIN base.levels_titles t ON t.id = c.level_id JOIN base.levels l ON l.id = t.id AND l.version = c.level_version LEFT JOIN base.users u ON u.id = t.author_user_id LEFT JOIN base.levels_plays p ON p.level_id = l.id LEFT JOIN base.levels_ratings r ON r.level_id = l.id LEFT JOIN base.levels_prize z ON z.level_id = t.id) AS l ORDER BY l.level_order ASC OFFSET {start} LIMIT {count}").ContinueWith(LevelManager.ParseSqlLevelDataMultipleAndTotal));
            }
            else
            {
                return DatabaseConnection.NewAsyncConnection((dbconnection) => dbconnection.ReadDataAsync($"SELECT COUNT(l.id) OVER() AS results, * FROM(SELECT DISTINCT ON(c.level_id) c.level_id AS id, t.title, l.version, t.author_user_id, COALESCE(u.username, 'Unknown') AS author_username, COALESCE(u.name_color, -14855017) AS author_name_color, t.title, l.description, l.publish, l.song_id, l.mode, l.seconds, l.gravity, l.alien, l.sfchm, l.snow, l.wind, l.items, l.health, l.king_of_the_hat, l.bg_image, l.level_data, l.last_updated, COALESCE(p.plays, 0) AS plays, COUNT(t.id) FILTER(WHERE r.rating = 'like') OVER(PARTITION BY t.id, l.version) AS likes, COUNT(t.id) FILTER(WHERE r.rating = 'dislike') OVER(PARTITION BY t.id, l.version) AS dislikes, c.level_order, CASE WHEN c.level_id IS NULL THEN false ELSE true END AS is_campaign, c.bronze_time, c.silver_time, c.gold_time, c.medals_required, c.season AS campaign_season, CASE WHEN z.level_id IS NULL THEN false ELSE true END AS has_prize, array_agg(ARRAY[z.part_type::text, z.part_id::text]) OVER(PARTITION BY t.id, l.version) AS prizes FROM base.campaigns c JOIN base.levels_titles t ON t.id = c.level_id JOIN base.levels l ON l.id = t.id AND l.version = c.level_version LEFT JOIN base.users u ON u.id = t.author_user_id LEFT JOIN base.levels_plays p ON p.level_id = l.id LEFT JOIN base.levels_ratings r ON r.level_id = l.id LEFT JOIN base.levels_prize z ON z.level_id = t.id WHERE c.season = {season}) AS l ORDER BY l.level_order ASC OFFSET {start} LIMIT {count}").ContinueWith(LevelManager.ParseSqlLevelDataMultipleAndTotal));
            }
        }

        public static Task<(uint results, IReadOnlyCollection<LevelData>)> GetBestTodayLevels(uint start = 0, uint count = uint.MaxValue, UserData user = null)
        {
            if (!user?.IsGuest ?? false)
            {
                if (user.HasPermissions(Permissions.ACCESS_SEE_UNPUBLISHED_LEVELS))
                {
                    return DatabaseConnection.NewAsyncConnection((dbconnection) => dbconnection.ReadDataAsync($"SELECT COUNT(l.id) OVER() AS results, * FROM(SELECT DISTINCT ON(t.id) t.id, l.version, t.author_user_id, COALESCE(u.username, 'Unknown') AS author_username, COALESCE(u.name_color, -14855017) AS author_name_color, t.title, l.description, l.publish, l.song_id, l.mode, l.seconds, l.gravity, l.alien, l.sfchm, l.snow, l.wind, l.items, l.health, l.king_of_the_hat, l.bg_image, l.level_data, l.last_updated, COALESCE(p.plays, 0) AS plays, COUNT(t.id) FILTER(WHERE r.rating = 'like') OVER(PARTITION BY t.id, l.version) AS likes, COUNT(t.id) FILTER(WHERE r.rating = 'dislike') OVER(PARTITION BY t.id, l.version) AS dislikes, CASE WHEN c.level_id IS NULL THEN false ELSE true END AS is_campaign, c.bronze_time, c.silver_time, c.gold_time, c.medals_required, c.season AS campaign_season, CASE WHEN z.level_id IS NULL THEN false ELSE true END AS has_prize, array_agg(ARRAY[z.part_type::text, z.part_id::text]) OVER(PARTITION BY t.id, l.version) AS prizes FROM base.levels_titles t JOIN base.levels l ON l.id = t.id LEFT JOIN base.users u ON u.id = t.author_user_id LEFT JOIN base.levels_plays p ON p.level_id = l.id LEFT JOIN base.levels_ratings r ON r.level_id = l.id LEFT JOIN base.campaigns c ON c.level_id = t.id LEFT JOIN base.levels_prize z ON z.level_id = t.id WHERE r.rated_on >= NOW() - INTERVAL '1 DAY' ORDER BY t.id, l.version DESC) AS l ORDER BY CASE WHEN l.dislikes = 0 THEN l.likes WHEN l.likes = 0 THEN -l.dislikes ELSE l.likes / l.dislikes END DESC OFFSET {start} LIMIT {count}").ContinueWith(LevelManager.ParseSqlLevelDataMultipleAndTotal));
                }
                else
                {
                    return DatabaseConnection.NewAsyncConnection((dbconnection) => dbconnection.ReadDataAsync($"SELECT COUNT(l.id) OVER() AS results, * FROM(SELECT DISTINCT ON(t.id) t.id, l.version, t.author_user_id, COALESCE(u.username, 'Unknown') AS author_username, COALESCE(u.name_color, -14855017) AS author_name_color, t.title, l.description, l.publish, l.song_id, l.mode, l.seconds, l.gravity, l.alien, l.sfchm, l.snow, l.wind, l.items, l.health, l.king_of_the_hat, l.bg_image, l.level_data, l.last_updated, COALESCE(p.plays, 0) AS plays, COUNT(t.id) FILTER(WHERE r.rating = 'like') OVER(PARTITION BY t.id, l.version) AS likes, COUNT(t.id) FILTER(WHERE r.rating = 'dislike') OVER(PARTITION BY t.id, l.version) AS dislikes, CASE WHEN c.level_id IS NULL THEN false ELSE true END AS is_campaign, c.bronze_time, c.silver_time, c.gold_time, c.medals_required, c.season AS campaign_season, CASE WHEN z.level_id IS NULL THEN false ELSE true END AS has_prize, array_agg(ARRAY[z.part_type::text, z.part_id::text]) OVER(PARTITION BY t.id, l.version) AS prizes FROM base.levels_titles t JOIN base.levels l ON l.id = t.id LEFT JOIN base.users u ON u.id = t.author_user_id LEFT JOIN base.levels_plays p ON p.level_id = l.id LEFT JOIN base.levels_ratings r ON r.level_id = l.id LEFT JOIN base.campaigns c ON c.level_id = t.id LEFT JOIN base.levels_prize z ON z.level_id = t.id WHERE (t.author_user_id = {user.Id} OR l.publish = '1') AND r.rated_on >= NOW() - INTERVAL '1 DAY' ORDER BY t.id, l.version DESC) AS l ORDER BY CASE WHEN l.dislikes = 0 THEN l.likes WHEN l.likes = 0 THEN -l.dislikes ELSE l.likes / l.dislikes END DESC OFFSET {start} LIMIT {count}").ContinueWith(LevelManager.ParseSqlLevelDataMultipleAndTotal));
                }
            }

            return DatabaseConnection.NewAsyncConnection((dbconnection) => dbconnection.ReadDataAsync($"SELECT COUNT(l.id) OVER() AS results, * FROM(SELECT DISTINCT ON(t.id) t.id, l.version, t.author_user_id, COALESCE(u.username, 'Unknown') AS author_username, COALESCE(u.name_color, -14855017) AS author_name_color, t.title, l.description, l.publish, l.song_id, l.mode, l.seconds, l.gravity, l.alien, l.sfchm, l.snow, l.wind, l.items, l.health, l.king_of_the_hat, l.bg_image, l.level_data, l.last_updated, COALESCE(p.plays, 0) AS plays, COUNT(t.id) FILTER(WHERE r.rating = 'like') OVER(PARTITION BY t.id, l.version) AS likes, COUNT(t.id) FILTER(WHERE r.rating = 'dislike') OVER(PARTITION BY t.id, l.version) AS dislikes, CASE WHEN c.level_id IS NULL THEN false ELSE true END AS is_campaign, c.bronze_time, c.silver_time, c.gold_time, c.medals_required, c.season AS campaign_season, CASE WHEN z.level_id IS NULL THEN false ELSE true END AS has_prize, array_agg(ARRAY[z.part_type::text, z.part_id::text]) OVER(PARTITION BY t.id, l.version) AS prizes FROM base.levels_titles t JOIN base.levels l ON l.id = t.id LEFT JOIN base.users u ON u.id = t.author_user_id LEFT JOIN base.levels_plays p ON p.level_id = l.id LEFT JOIN base.levels_ratings r ON r.level_id = l.id LEFT JOIN base.campaigns c ON c.level_id = t.id LEFT JOIN base.levels_prize z ON z.level_id = t.id WHERE l.publish = '1' AND r.rated_on >= NOW() - INTERVAL '1 DAY' ORDER BY t.id, l.version DESC) AS l ORDER BY CASE WHEN l.dislikes = 0 THEN l.likes WHEN l.likes = 0 THEN -l.dislikes ELSE l.likes / l.dislikes END DESC OFFSET {start} LIMIT {count}").ContinueWith(LevelManager.ParseSqlLevelDataMultipleAndTotal));
        }

        public static Task<(uint levelId, uint plays)> AddPlaysAsync(uint levelId, uint plays)
        {
            return DatabaseConnection.NewAsyncConnection((dbconnection) => dbconnection.ReadDataAsync($"INSERT INTO base.levels_plays(level_id, plays) VALUES({levelId}, {plays}) ON CONFLICT(level_id) DO UPDATE SET plays = levels_plays.plays + excluded.plays RETURNING level_id, plays").ContinueWith(LevelManager.ParseSqlAddPlays));
        }

        public static Task RateLevelAsync(uint levelId, uint userId, int rating)
        {
            if (rating != -1 && rating != 1)
            {
                return Task.FromException(new ArgumentException(nameof(rating)));
            }

            return DatabaseConnection.NewAsyncConnection((dbconnection) => dbconnection.ExecuteNonQueryAsync($"INSERT INTO base.levels_ratings(level_id, user_id, rating) VALUES({levelId}, {userId}, {(rating == 1 ? "like" : "dislike")}::base.level_rating) ON CONFLICT(level_id, user_id) DO UPDATE SET rating = excluded.rating, updated_on = NOW()"));
        }

        public static Task<uint> TransferLevelAsync(uint levelId, uint authorId, uint toUserId, string title)
        {
            return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"WITH pm AS (INSERT INTO base.pms(to_user_id, from_user_id, type) VALUES({toUserId}, {authorId}, 'level') RETURNING id, from_user_id), level AS(SELECT id, title FROM base.levels_titles WHERE id = {levelId} AND author_user_id = (SELECT from_user_id FROM pm) LIMIT 1) INSERT INTO base.transfers_level(id, level_id, title) SELECT p.id, l.id, l.title FROM pm p LEFT JOIN level l ON TRUE RETURNING id").ContinueWith(LevelManager.ParseSqlTransferLevel));
        }

        public static Task<IReadOnlyCollection<LevelData>> SearchLevels(string mode, string sort, string dir, string searchStr, UserData user = null)
        {
            StringBuilder query = new StringBuilder();
            if (!user?.IsGuest ?? false)
            {
                if (user.HasPermissions(Permissions.ACCESS_SEE_UNPUBLISHED_LEVELS))
                {
                    query.Append($"SELECT COUNT(l.id) OVER() AS results, * FROM(SELECT DISTINCT ON(t.id) t.id, l.version, t.author_user_id, COALESCE(u.username, 'Unknown') AS author_username, COALESCE(u.name_color, -14855017) AS author_name_color, t.title, l.description, l.publish, l.song_id, l.mode, l.seconds, l.gravity, l.alien, l.sfchm, l.snow, l.wind, l.items, l.health, l.king_of_the_hat, l.bg_image, l.level_data, l.last_updated, COALESCE(p.plays, 0) AS plays, COUNT(t.id) FILTER(WHERE r.rating = 'like') OVER(PARTITION BY t.id, l.version) AS likes, COUNT(t.id) FILTER(WHERE r.rating = 'dislike') OVER(PARTITION BY t.id, l.version) AS dislikes, CASE WHEN c.level_id IS NULL THEN false ELSE true END AS is_campaign, c.bronze_time, c.silver_time, c.gold_time, c.medals_required, c.season AS campaign_season, CASE WHEN z.level_id IS NULL THEN false ELSE true END AS has_prize, array_agg(ARRAY[z.part_type::text, z.part_id::text]) OVER(PARTITION BY t.id, l.version) AS prizes FROM base.levels_titles t JOIN base.levels l ON l.id = t.id LEFT JOIN base.users u ON u.id = t.author_user_id LEFT JOIN base.levels_plays p ON p.level_id = l.id LEFT JOIN base.levels_ratings r ON r.level_id = l.id LEFT JOIN base.campaigns c ON c.level_id = t.id LEFT JOIN base.levels_prize z ON z.level_id = t.id WHERE ");
                }
                else
                {
                    query.Append($"SELECT COUNT(l.id) OVER() AS results, * FROM(SELECT DISTINCT ON(t.id) t.id, l.version, t.author_user_id, COALESCE(u.username, 'Unknown') AS author_username, COALESCE(u.name_color, -14855017) AS author_name_color, t.title, l.description, l.publish, l.song_id, l.mode, l.seconds, l.gravity, l.alien, l.sfchm, l.snow, l.wind, l.items, l.health, l.king_of_the_hat, l.bg_image, l.level_data, l.last_updated, COALESCE(p.plays, 0) AS plays, COUNT(t.id) FILTER(WHERE r.rating = 'like') OVER(PARTITION BY t.id, l.version) AS likes, COUNT(t.id) FILTER(WHERE r.rating = 'dislike') OVER(PARTITION BY t.id, l.version) AS dislikes, CASE WHEN c.level_id IS NULL THEN false ELSE true END AS is_campaign, c.bronze_time, c.silver_time, c.gold_time, c.medals_required, c.season AS campaign_season, CASE WHEN z.level_id IS NULL THEN false ELSE true END AS has_prize, array_agg(ARRAY[z.part_type::text, z.part_id::text]) OVER(PARTITION BY t.id, l.version) AS prizes FROM base.levels_titles t JOIN base.levels l ON l.id = t.id LEFT JOIN base.users u ON u.id = t.author_user_id LEFT JOIN base.levels_plays p ON p.level_id = l.id LEFT JOIN base.levels_ratings r ON r.level_id = l.id LEFT JOIN base.campaigns c ON c.level_id = t.id LEFT JOIN base.levels_prize z ON z.level_id = t.id WHERE (l.publish = '1' OR t.author_user_id = {user.Id}) AND ");
                }
            }
            else
            {
                query.Append($"SELECT COUNT(l.id) OVER() AS results, * FROM(SELECT DISTINCT ON(t.id) t.id, l.version, t.author_user_id, COALESCE(u.username, 'Unknown') AS author_username, COALESCE(u.name_color, -14855017) AS author_name_color, t.title, l.description, l.publish, l.song_id, l.mode, l.seconds, l.gravity, l.alien, l.sfchm, l.snow, l.wind, l.items, l.health, l.king_of_the_hat, l.bg_image, l.level_data, l.last_updated, COALESCE(p.plays, 0) AS plays, COUNT(t.id) FILTER(WHERE r.rating = 'like') OVER(PARTITION BY t.id, l.version) AS likes, COUNT(t.id) FILTER(WHERE r.rating = 'dislike') OVER(PARTITION BY t.id, l.version) AS dislikes, CASE WHEN c.level_id IS NULL THEN false ELSE true END AS is_campaign, c.bronze_time, c.silver_time, c.gold_time, c.medals_required, c.season AS campaign_season, CASE WHEN z.level_id IS NULL THEN false ELSE true END AS has_prize, array_agg(ARRAY[z.part_type::text, z.part_id::text]) OVER(PARTITION BY t.id, l.version) AS prizes FROM base.levels_titles t JOIN base.levels l ON l.id = t.id LEFT JOIN base.users u ON u.id = t.author_user_id LEFT JOIN base.levels_plays p ON p.level_id = l.id LEFT JOIN base.levels_ratings r ON r.level_id = l.id LEFT JOIN base.campaigns c ON c.level_id = t.id LEFT JOIN base.levels_prize z ON z.level_id = t.id WHERE l.publish = '1' AND ");
            }

            if (mode == "title")
            {
                query.Append("t.title ILIKE '%' || @searchStr || '%' ");
            }
            else if (mode == "user")
            {
                query.Append("u.username ILIKE @searchStr ");
            }
            else
            {
                throw new ArgumentException(nameof(mode));
            }
            
            query.Append("ORDER BY t.id, l.version DESC) AS l ");

            if (sort == "date")
            {
                query.Append("ORDER BY l.last_updated ");
            }
            else if (sort == "alphabetical")
            {
                query.Append("ORDER BY l.title ");
            }
            else if (sort == "rating")
            {
                query.Append("ORDER BY CASE WHEN l.dislikes = 0 THEN l.likes WHEN l.likes = 0 THEN -l.dislikes ELSE l.likes / l.dislikes END ");
            }
            else if (sort == "popularity")
            {
                query.Append("ORDER BY l.plays ");
            }
            else
            {
                throw new ArgumentException(nameof(sort));
            }

            if (dir == "desc")
            {
                query.Append("DESC ");
            }
            else if (dir == "asc")
            {
                query.Append("ASC ");
            }
            else
            {
                throw new ArgumentException(nameof(dir));
            }

            return DatabaseConnection.NewAsyncConnection((dbConnection) =>
            {
                dbConnection.AddParamWithValue("searchStr", searchStr);
                return dbConnection.ReadDataUnsafeAsync(query.ToString()).ContinueWith(LevelManager.ParseSqlLevelDataMultiple);
            });
        }

        private static bool ParseSqlDeleteLevel(Task<DbDataReader> task)
        {
            if (task.IsCompletedSuccessfully)
            {
                DbDataReader reader = task.Result;
                if (reader?.Read() ?? false)
                {
                    uint levelId = (uint)(int)reader["id"];

                    //sDELETE CACHE

                    return true;
                }
            }
            else if (task.IsFaulted)
            {
                LevelManager.Logger.Error("Failed to delete level from sql", task.Exception);
            }

            return false;
        }

        private static uint ParseSqlCountMyLevels(Task<DbDataReader> task)
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
                LevelManager.Logger.Error("Failed to count users level from sql", task.Exception);
            }

            return 0u;
        }

        private static LevelData ParseSqlLevelData(Task<DbDataReader> task)
        {
            if (task.IsCompletedSuccessfully)
            {
                DbDataReader reader = task.Result;
                if (reader?.Read() ?? false)
                {
                    return new LevelData(reader);
                }
            }
            else if (task.IsFaulted)
            {
                LevelManager.Logger.Error($"Failed to load {nameof(LevelData)} from sql", task.Exception);
            }

            return null;
        }

        private static uint ParseSqlSaveLevel(Task<DbDataReader> task)
        {
            if (task.IsCompletedSuccessfully)
            {
                DbDataReader reader = task.Result;
                if (reader?.Read() ?? false)
                {
                    return (uint)(int)reader["id"];
                }
            }
            else if (task.IsFaulted)
            {
                LevelManager.Logger.Error("Failed to to save level to sql", task.Exception);
            }

            return 0u;
        }

        private static IReadOnlyCollection<LevelData> ParseSqlLevelDataMultiple(Task<DbDataReader> task)
        {
            List<LevelData> levels = new List<LevelData>();
            if (task.IsCompletedSuccessfully)
            {
                DbDataReader reader = task.Result;
                while (reader?.Read() ?? false)
                {
                    levels.Add(new LevelData(reader));
                }
            }
            else if (task.IsFaulted)
            {
                LevelManager.Logger.Error($"Failed to load {nameof(LevelData)} from sql", task.Exception);
            }

            return levels;
        }

        private static (uint results, IReadOnlyCollection<LevelData>) ParseSqlLevelDataMultipleAndTotal(Task<DbDataReader> task)
        {
            uint results = 0;
            List<LevelData> levels = new List<LevelData>();
            if (task.IsCompletedSuccessfully)
            {
                DbDataReader reader = task.Result;
                while (reader?.Read() ?? false)
                {
                    if (results == 0)
                    {
                        results = (uint)(long)reader["results"];
                    }

                    levels.Add(new LevelData(reader));
                }
            }
            else if (task.IsFaulted)
            {
                LevelManager.Logger.Error($"Failed to load {nameof(LevelData)} from sql", task.Exception);
            }

            return (results, levels);
        }

        private static (uint levelId, uint plays) ParseSqlAddPlays(Task<DbDataReader> task)
        {
            if (task.IsCompletedSuccessfully)
            {
                DbDataReader reader = task.Result;
                while (reader?.Read() ?? false)
                {
                    return ((uint)(int)reader["level_id"], (uint)(int)reader["plays"]);
                }
            }
            else if (task.IsFaulted)
            {
                LevelManager.Logger.Error($"Failed to update levels play count to sql", task.Exception);
            }

            return (0, 0);
        }

        private static uint ParseSqlTransferLevel(Task<DbDataReader> task)
        {
            if (task.IsCompletedSuccessfully)
            {
                DbDataReader reader = task.Result;
                while (reader?.Read() ?? false)
                {
                    return (uint)(int)reader["id"];
                }
            }
            else if (task.IsFaulted)
            {
                LevelManager.Logger.Error($"Failed to save transfer level to sql", task.Exception);
            }

            return 0;
        }
    }
}
