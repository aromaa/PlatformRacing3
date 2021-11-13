using System.Data.Common;
using Microsoft.Extensions.Logging;
using Npgsql;
using PlatformRacing3.Common.Database;
using PlatformRacing3.Common.Utils;

namespace PlatformRacing3.Common.Block;

public sealed class BlockManager
{
	private static readonly ILogger<BlockManager> logger = LoggerUtil.LoggerFactory.CreateLogger<BlockManager>();

	public static Task<BlockData> GetBlockAsync(uint blockId)
	{
		if (blockId == 0)
		{
			throw new ArgumentException(null, nameof(blockId));
		}

		return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT t.id, b.version, t.author_user_id, t.title, t.category, b.description, b.image_data, b.settings, b.last_updated FROM base.blocks_titles t LEFT JOIN LATERAL(SELECT b.* FROM base.blocks b WHERE b.id = t.id ORDER BY b.version DESC LIMIT 1) b ON TRUE WHERE t.id = {blockId} LIMIT 1").ContinueWith(BlockManager.ParseSqlGetBlock));
	}

	public static Task<BlockData> GetBlockAsync(uint userId, string title, string category)
	{
		return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT t.id, b.version, t.author_user_id, t.title, t.category, b.description, b.image_data, b.settings, b.last_updated FROM base.blocks_titles t LEFT JOIN LATERAL(SELECT b.* FROM base.blocks b WHERE b.id = t.id ORDER BY b.version DESC LIMIT 1) b ON TRUE WHERE t.author_user_id = {userId} AND t.title ILIKE {title} AND t.category ILIKE {category} LIMIT 1").ContinueWith(BlockManager.ParseSqlGetBlock));
	}

	public static Task<List<BlockData>> GetBlocksAsync(params uint[] blockIds)
	{
		return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT t.id, b.version, t.author_user_id, t.title, t.category, b.description, b.image_data, b.settings, b.last_updated FROM base.blocks_titles t LEFT JOIN LATERAL(SELECT b.* FROM base.blocks b WHERE b.id = t.id ORDER BY b.version DESC LIMIT 1) b ON TRUE WHERE t.id = ANY({blockIds}) LIMIT {blockIds.Length}").ContinueWith(BlockManager.ParseSqlGetBlocks));
	}

	public static Task<uint> CountMyBlocksAsync(uint userId, string category)
	{
		if (userId == 0)
		{
			throw new ArgumentException(null, nameof(userId));
		}

		//TODO: CACHE
            
		if (category == "default-all-blocks")
		{
			return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT COUNT(id) AS count FROM base.blocks_titles WHERE author_user_id = {userId}").ContinueWith(BlockManager.ParseSqlReadCountMyBlocks));
		}
		else if (category == "default-all-blocks-without-category")
		{
			return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT COUNT(id) AS count FROM base.blocks_titles WHERE author_user_id = {userId} AND category = ''").ContinueWith(BlockManager.ParseSqlReadCountMyBlocks));
		}
		else if (category.StartsWith("category-"))
		{
			return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT COUNT(id) AS count FROM base.blocks_titles WHERE author_user_id = {userId} AND category ILIKE {category["category-".Length..]}").ContinueWith(BlockManager.ParseSqlReadCountMyBlocks));
		}
		else
		{
			throw new ArgumentException(null, nameof(category));
		}
	}

	public static Task<HashSet<string>> GetMyCategorysAsync(uint userId)
	{
		if (userId == 0)
		{
			throw new ArgumentException(null, nameof(userId));
		}

		return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT DISTINCT ON (lower(category)) category FROM base.blocks_titles WHERE author_user_id = {userId} AND category != '' ORDER BY lower(category)").ContinueWith(BlockManager.ParseSqlGetCategorys));
	}

	public static Task<HashSet<uint>> GetMyBlocksAsync(uint userId, string category, uint start = 0, uint count = uint.MaxValue)
	{
		if (userId == 0)
		{
			throw new ArgumentException(null, nameof(userId));
		}
            
		if (category == "default-all-blocks")
		{
			return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT t.id FROM base.blocks_titles t LEFT JOIN LATERAL(SELECT b.* FROM base.blocks b WHERE b.id = t.id ORDER BY b.version DESC LIMIT 1) b ON TRUE WHERE t.author_user_id = {userId} ORDER BY b.last_updated DESC OFFSET {start} LIMIT {count}").ContinueWith(BlockManager.ParseSqlGetMyBlocks));
		}
		else if (category == "default-all-blocks-without-category")
		{
			return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT t.id FROM base.blocks_titles t LEFT JOIN LATERAL(SELECT b.* FROM base.blocks b WHERE b.id = t.id ORDER BY b.version DESC LIMIT 1) b ON TRUE WHERE t.author_user_id = {userId} AND t.category = '' ORDER BY b.last_updated DESC OFFSET {start} LIMIT {count}").ContinueWith(BlockManager.ParseSqlGetMyBlocks));
		}
		else if (category.StartsWith("category-"))
		{
			return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT t.id FROM base.blocks_titles t LEFT JOIN LATERAL(SELECT b.* FROM base.blocks b WHERE b.id = t.id ORDER BY b.version DESC LIMIT 1) b ON TRUE WHERE t.author_user_id = {userId} AND t.category ILIKE {category["category-".Length..]} ORDER BY b.last_updated DESC OFFSET {start} LIMIT {count}").ContinueWith(BlockManager.ParseSqlGetMyBlocks));
		}
		else
		{
			throw new ArgumentException(null, nameof(category));
		}
	}

	public static Task<bool> SaveBlockAsync(uint userId, string title, string category, string description, string imageData, string settings)
	{
		if (userId == 0)
		{
			throw new ArgumentException(null, nameof(userId));
		}

		return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"WITH insertTitle AS (INSERT INTO base.blocks_titles(title, category, author_user_id) VALUES({title}, {category}, {userId}) ON CONFLICT(lower(title::text), lower(category::text), author_user_id) DO UPDATE SET title = blocks_titles.title RETURNING id) INSERT INTO base.blocks(id, version, description, image_data, settings) SELECT (SELECT id FROM insertTitle), COALESCE(MAX(version) + 1, 1), {description}, {imageData}, {settings} FROM base.blocks WHERE id = (SELECT id FROM insertTitle) RETURNING id").ContinueWith(BlockManager.ParseSqlSaveBlock));
	}

	public static Task<bool> DeleteBlockAsync(uint blockId, uint authorId = default)
	{
		if (authorId != default)
		{
			return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"WITH deleted AS (DELETE FROM base.blocks_titles WHERE id = {blockId} AND author_user_id = {authorId} RETURNING id, title, category, author_user_id) INSERT INTO base.blocks_deleted(id, title, category, author_user_id) SELECT id, title, category, author_user_id FROM deleted RETURNING ID").ContinueWith(BlockManager.ParseSqlDeleteBlock));
		}
		else
		{
			return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"WITH deleted AS (DELETE FROM base.blocks_titles WHERE id = {blockId} AND author_user_id = {authorId} RETURNING id, title, category, author_user_id) INSERT INTO base.blocks_deleted(id, title, category, author_user_id) SELECT id, title, category, author_user_id FROM deleted RETURNING ID").ContinueWith(BlockManager.ParseSqlDeleteBlock));
		}
	}

	public static Task<uint> TransferBlockAsync(uint blockId, uint authorId, uint toUserId, string title)
	{
		return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"WITH pm AS(INSERT INTO base.pms(to_user_id, from_user_id, type) VALUES({toUserId}, {authorId}, 'block') RETURNING from_user_id, id), block AS(SELECT id, title FROM base.blocks_titles WHERE id = {blockId} AND author_user_id = (SELECT from_user_id FROM pm) LIMIT 1) INSERT INTO base.transfers_block(id, block_id, title) SELECT p.id, b.id, b.title FROM pm p LEFT JOIN block b ON TRUE RETURNING id").ContinueWith(BlockManager.ParseSqlTransferBlock));
	}

	private static BlockData ParseSqlGetBlock(Task<NpgsqlDataReader> task)
	{
		if (task.IsCompletedSuccessfully)
		{
			DbDataReader reader = task.Result;
			if (reader?.Read() ?? false)
			{
				return new BlockData(reader);
			}
		}
		else if (task.IsFaulted)
		{
			BlockManager.logger.LogError(EventIds.BlockLoadFailed, task.Exception, $"Failed to load {nameof(BlockData)} from sql");
		}

		return null;
	}

	private static List<BlockData> ParseSqlGetBlocks(Task<NpgsqlDataReader> task)
	{
		List<BlockData> blocks = new();
		if (task.IsCompletedSuccessfully)
		{
			DbDataReader reader = task.Result;
			while (reader?.Read() ?? false)
			{
				blocks.Add(new BlockData(reader));
			}
		}
		else if (task.IsFaulted)
		{
			BlockManager.logger.LogError(EventIds.BlockLoadFailed, task.Exception, $"Failed to load {nameof(BlockData)} from sql");
		}

		return blocks;
	}

	private static uint ParseSqlReadCountMyBlocks(Task<NpgsqlDataReader> task)
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
			BlockManager.logger.LogError(EventIds.BlockLoadFailed, task.Exception, "Failed to count users block count from sql");
		}

		return 0u;
	}

	private static HashSet<string> ParseSqlGetCategorys(Task<NpgsqlDataReader> task)
	{
		HashSet<string> categories = new();
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
			BlockManager.logger.LogError(EventIds.BlockLoadFailed, task.Exception, "Failed to load users block categories from sql");
		}

		return categories;
	}

	private static HashSet<uint> ParseSqlGetMyBlocks(Task<NpgsqlDataReader> task)
	{
		HashSet<uint> blocks = new();
		if (task.IsCompletedSuccessfully)
		{
			DbDataReader reader = task.Result;
			while (reader?.Read() ?? false)
			{
				blocks.Add((uint)(int)reader["id"]);
			}
		}
		else if (task.IsFaulted)
		{
			BlockManager.logger.LogError(EventIds.BlockLoadFailed, task.Exception, "Failed to load user blocks from sql");
		}

		return blocks;
	}

	private static bool ParseSqlSaveBlock(Task<NpgsqlDataReader> task)
	{
		if (task.IsCompletedSuccessfully)
		{
			DbDataReader reader = task.Result;
			if (reader?.Read() ?? false)
			{
				uint blockId = (uint)(int)reader["id"];

				return blockId > 0;
			}
		}
		else if (task.IsFaulted)
		{
			BlockManager.logger.LogError(EventIds.BlockSaveFailed, task.Exception, "Failed to save block to sql");
		}

		return false;
	}

	private static bool ParseSqlDeleteBlock(Task<NpgsqlDataReader> task)
	{
		if (task.IsCompletedSuccessfully)
		{
			DbDataReader reader = task.Result;
			if (reader?.Read() ?? false)
			{
				uint blockId = (uint)(int)reader["id"];

				return blockId > 0;
			}
		}
		else if (task.IsFaulted)
		{
			BlockManager.logger.LogError(EventIds.BlockSaveFailed, task.Exception, "Failed to delete block from sql");
		}

		return false;
	}

	private static uint ParseSqlTransferBlock(Task<NpgsqlDataReader> task)
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
			BlockManager.logger.LogError(EventIds.BlockSaveFailed, task.Exception, "Failed transfer block as sql");
		}

		return 0;
	}
}