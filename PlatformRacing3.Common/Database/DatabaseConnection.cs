using System;
using System.Buffers;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Npgsql;
using PlatformRacing3.Common.Campaign;
using PlatformRacing3.Common.Config;
using PlatformRacing3.Common.Level;

namespace PlatformRacing3.Common.Database
{
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

        public DbDataReader ReadData(FormattableString query)
        {
            this.PrepareQuery(query);

            this.Command.Prepare();

            return this.Command.ExecuteReader();
        }
        
        public Task<NpgsqlDataReader> ReadDataAsync(FormattableString query)
        {
            this.PrepareQuery(query);

            this.Command.Prepare();

            return this.Command.ExecuteReaderAsync();
        }

        public Task<NpgsqlDataReader> ReadDataUnsafeAsync(string query)
        {
            this.Command.CommandText = query;

            return this.Command.ExecuteReaderAsync();
        }

        public Task<int> ExecuteNonQueryAsync(FormattableString query)
        {
            this.PrepareQuery(query);

            this.Command.Prepare();

            return this.Command.ExecuteNonQueryAsync();
        }

        public int ExecuteNonQuery(FormattableString query)
        {
            this.PrepareQuery(query);

            this.Command.Prepare();

            return this.Command.ExecuteNonQuery();
        }

        public object ExecuteScalar(FormattableString query)
        {
            this.PrepareQuery(query);

            this.Command.Prepare();

            return this.Command.ExecuteScalar();
        }

        public void AddParamWithValue(string name, object value)
        {
            this.Command.Parameters.AddWithValue(name, value);
        }

        //This methods does something horrible...
        public void PrepareQuery(FormattableString query)
        {
            this.Command.Parameters.Clear();

            object[] @params = ArrayPool<object>.Shared.Rent(query.ArgumentCount);

            try
            {
                for (int i = 0; i < query.ArgumentCount; i++)
                {
                    object argument = query.GetArgument(i);
                    if (argument is ushort @short)
                    {
                        argument = Convert.ToInt32(@short);
                    }
                    else if (argument is uint @uint)
                    {
                        argument = Convert.ToInt64(@uint);
                    }
                    else if (argument is ulong @ulong)
                    {
                        argument = Unsafe.As<ulong, long>(ref @ulong);
                    }
                    else if (argument is uint[] uintArray)
                    {
                        argument = MemoryMarshal.Cast<uint, int>(uintArray).ToArray();
                    }

                    this.Command.Parameters.AddWithValue(i.ToString(), argument);

                    @params[i] = "@" + i;
                }

                this.SetQuery(string.Format(DatabaseIFormatProvider.Instance, query.Format, @params));
            }
            finally
            {
                ArrayPool<object>.Shared.Return(@params);
            }
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

        public static Task NewAsyncConnection(Func<DatabaseConnection, Task> func)
        {
            DatabaseConnection dbConnection = new();

            try
            {
                Task task = func.Invoke(dbConnection);

                Task.WhenAll(task).ContinueWith((task_) => dbConnection.Dispose()); //All is done, dispose

                return task;
            }
            catch (Exception ex)
            {
                dbConnection.Dispose();

                return Task.FromException(ex);
            }
        }

        public static Task<TNewResult> NewAsyncConnection<TNewResult>(Func<DatabaseConnection, Task<TNewResult>> func)
        {
            DatabaseConnection dbConnection = new();

            try
            {
                Task<TNewResult> task = func.Invoke(dbConnection);
                
                Task.WhenAll(task).ContinueWith((task_) => dbConnection.Dispose()); //All is done, dispose

                return task;
            }
            catch(Exception ex)
            {
                dbConnection.Dispose();

                return Task.FromException<TNewResult>(ex);
            }
        }
    }
}
