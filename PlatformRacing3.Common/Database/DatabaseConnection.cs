using System.Data.Common;
using System.Runtime.CompilerServices;
using Npgsql;
using NpgsqlTypes;
using PlatformRacing3.Common.Campaign;
using PlatformRacing3.Common.Config;
using PlatformRacing3.Common.Level;

namespace PlatformRacing3.Common.Database;

public class DatabaseConnection : IDisposable
{
	private static string ConnectionString;

	private NpgsqlConnection Connection;
	private NpgsqlCommand Command;

	static DatabaseConnection()
	{
		NpgsqlConnection.GlobalTypeMapper.MapEnum<LevelMode>();
		NpgsqlConnection.GlobalTypeMapper.MapEnum<CampaignPrizeType>();
	}

	public static void Init(IDatabaseConfig dbConfig)
	{
		DatabaseConnection.ConnectionString = $"Host={dbConfig.DatabaseHost};Port={dbConfig.DatabasePort};Username={dbConfig.DatabaseUser};Password={dbConfig.DatabasePass};Database={dbConfig.DatabaseName}";
		DatabaseConnection.TestConnection();
	}

	private static void TestConnection()
	{
		using (NpgsqlConnection dbConnection = new(DatabaseConnection.ConnectionString))
		{
			dbConnection.Open();

			using (NpgsqlCommand command = new("SELECT 1", dbConnection))
			{
				command.ExecuteReader();
			}
		}
	}

	public DatabaseConnection()
	{
		this.Connection = new NpgsqlConnection(DatabaseConnection.ConnectionString);
		this.Command = new NpgsqlCommand(null, this.Connection);

		this.Connection.Open();
	}

	internal void ResetState()
	{
		this.Command.Parameters.Clear();
	}

	public DbDataReader ReadData([InterpolatedStringHandlerArgument("")] ref DatabaseInterpolatedStringHandler handler)
	{
		this.PrepareQuery(ref handler);

		this.Command.Prepare();

		return this.Command.ExecuteReader();
	}
        
	public Task<NpgsqlDataReader> ReadDataAsync([InterpolatedStringHandlerArgument("")] ref DatabaseInterpolatedStringHandler handler)
	{
		this.PrepareQuery(ref handler);

		this.Command.Prepare();

		return this.Command.ExecuteReaderAsync();
	}

	public Task<NpgsqlDataReader> ReadDataUnsafeAsync(string query)
	{
		this.Command.CommandText = query;

		return this.Command.ExecuteReaderAsync();
	}

	public Task<int> ExecuteNonQueryAsync([InterpolatedStringHandlerArgument("")] ref DatabaseInterpolatedStringHandler handler)
	{
		this.PrepareQuery(ref handler);

		this.Command.Prepare();

		return this.Command.ExecuteNonQueryAsync();
	}

	public int ExecuteNonQuery([InterpolatedStringHandlerArgument("")] ref DatabaseInterpolatedStringHandler handler)
	{
		this.PrepareQuery(ref handler);

		this.Command.Prepare();

		return this.Command.ExecuteNonQuery();
	}

	public object ExecuteScalar([InterpolatedStringHandlerArgument("")] ref DatabaseInterpolatedStringHandler handler)
	{
		this.PrepareQuery(ref handler);

		this.Command.Prepare();

		return this.Command.ExecuteScalar();
	}

	public void AddParamWithValue(string name, object value)
	{
		this.Command.Parameters.AddWithValue(name, value);
	}

	public void AddParamWithValue(object value)
	{
		this.Command.Parameters.AddWithValue(value);
	}

	public void AddParamWithValue(object value, NpgsqlDbType type)
	{
		this.Command.Parameters.AddWithValue(type, value);
	}

	public void PrepareQuery(ref DatabaseInterpolatedStringHandler handler)
	{
		this.SetQuery(handler.Text);
	}

	private void SetQuery(string query)
	{
		this.Command.CommandText = query;
	}

	public void Dispose()
	{
		this.Connection.Dispose();
		this.Command.Dispose();
	}

	public static async Task NewAsyncConnection(Func<DatabaseConnection, Task> func)
	{
		DatabaseConnection dbConnection = new();

		try
		{
			await func.Invoke(dbConnection);
		}
		finally
		{
			dbConnection.Dispose();
		}
	}

	public static async Task<TNewResult> NewAsyncConnection<TNewResult>(Func<DatabaseConnection, Task<TNewResult>> func)
	{
		DatabaseConnection dbConnection = new();

		try
		{
			return await func.Invoke(dbConnection);
		}
		finally
		{
			dbConnection.Dispose();
		}
	}
}