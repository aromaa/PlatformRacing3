using Npgsql;
using NpgsqlTypes;
using Platform_Racing_3_Common.Campaign;
using Platform_Racing_3_Common.Config;
using Platform_Racing_3_Common.Level;
using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Platform_Racing_3_Common.Database
{
    public class DatabaseConnection : IDisposable
    {
        private static string ConnectionString = null;
        private static SshClient SshClient = null;

        private NpgsqlConnection Connection;
        private NpgsqlCommand Command;

        static DatabaseConnection()
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<LevelMode>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<CampaignPrizeType>();
        }

        public static void Init(IDatabaseConfig dbConfig)
        {
            if (dbConfig.DatabaseUseSsh)
            {
                DatabaseConnection.SshClient = new SshClient(dbConfig.DatabaseHost, dbConfig.DatabaseSshUser, new PrivateKeyFile(dbConfig.DatabaseSshKey));
                DatabaseConnection.SshClient.Connect();

                ForwardedPortLocal forward = new ForwardedPortLocal("127.0.0.1", "127.0.0.1", dbConfig.DatabasePort);

                DatabaseConnection.SshClient.AddForwardedPort(forward);

                forward.Start();

                DatabaseConnection.ConnectionString = $"Host={forward.BoundHost};Port={forward.BoundPort};Username={dbConfig.DatabaseUser};Password={dbConfig.DatabasePass};Database={dbConfig.DatabaseName}";
            }
            else
            {
                DatabaseConnection.ConnectionString = $"Host={dbConfig.DatabaseHost};Username={dbConfig.DatabaseUser};Password={dbConfig.DatabasePass};Database={dbConfig.DatabaseName}";
            }
            
            DatabaseConnection.TestConnection();
        }

        private static void TestConnection()
        {
            using (NpgsqlConnection dbConnection = new NpgsqlConnection(DatabaseConnection.ConnectionString))
            {
                dbConnection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand("SELECT 1", dbConnection))
                {
                    command.ExecuteReader();
                }
            }
        }

        public DatabaseConnection()
        {
            this.Connection = new NpgsqlConnection(DatabaseConnection.ConnectionString);
            this.Command = new NpgsqlCommand(null, this.Connection);
        }

        public DbDataReader ReadData(FormattableString query)
        {
            this.PrepareQuery(query);

            this.Connection.Open();
            return this.Command.ExecuteReader();
        }
        
        public Task<DbDataReader> ReadDataAsync(FormattableString query)
        {
            this.PrepareQuery(query);
            
            return this.Connection.OpenAsync().ContinueWith((task) => this.Command.ExecuteReaderAsync()).Unwrap(); //Unwrap should be fine? Or is there better way?
        }

        public Task<DbDataReader> ReadDataUnsafeAsync(string query)
        {
            this.Command.CommandText = query;

            return this.Connection.OpenAsync().ContinueWith((task) => this.Command.ExecuteReaderAsync()).Unwrap(); //Unwrap should be fine? Or is there better way?
        }

        public Task<int> ExecuteNonQueryAsync(FormattableString query)
        {
            this.PrepareQuery(query);

            return this.Connection.OpenAsync().ContinueWith((task) => this.Command.ExecuteNonQueryAsync()).Unwrap(); //Unwrap should be fine? Or is there better way?
        }

        public void AddParamWithValue(string name, object value)
        {
            this.Command.Parameters.AddWithValue(name, value);
        }

        //This methods does something horrible...
        public void PrepareQuery(FormattableString query)
        {
            NpgsqlParameter[] sqlParams = query.GetArguments().Select((value, position) => new NpgsqlParameter(position.ToString(), value)).ToArray();
            
            this.SetQuery(string.Format(DatabaseIFormatProvider.Instance, query.Format, sqlParams));

            this.Command.Parameters.AddRange(sqlParams.Where((p) => !Object.ReferenceEquals(p.Value, DatabaseCustomFormatter.DeleteParamReference)).ToArray());
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
            DatabaseConnection dbConnection = new DatabaseConnection();

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
            DatabaseConnection dbConnection = new DatabaseConnection();

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
