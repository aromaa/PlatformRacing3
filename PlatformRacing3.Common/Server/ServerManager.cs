using Microsoft.Extensions.Caching.Memory;
using Platform_Racing_3_Common.Database;
using Platform_Racing_3_Common.Redis;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Npgsql;
using Platform_Racing_3_Common.Utils;

namespace Platform_Racing_3_Common.Server
{
    public sealed class ServerManager
    {
	    public const uint SERVER_STATUS_TIMEOUT = 3;
        public const string SERVER_STATUS_TIMEOUT_MESSAGE = "Offline";

        private readonly ILogger<ServerManager> logger;
        
        private ConcurrentDictionary<uint, ServerDetails> Servers;

        public ServerManager(ILogger<ServerManager> logger)
        {
            this.logger = logger;

            this.Servers = new ConcurrentDictionary<uint, ServerDetails>();
        }

        public async Task LoadServersAsync()
        {
            await RedisConnection.GetConnectionMultiplexer().GetSubscriber().SubscribeAsync("ServerStatusUpdated", this.RedisServerStatusUpdate, CommandFlags.FireAndForget);

            using (DatabaseConnection dbConnection = new())
            {
                await dbConnection.ReadDataAsync($"SELECT id, name, ip, port FROM base.servers ORDER BY id").ContinueWith(this.ParseSqlServers);
            }

            foreach (ServerDetails server in this.Servers.Values)
            {
                RedisValue value = RedisConnection.GetDatabase().StringGet($"server-status:{server.Id}");
                if (value.HasValue)
                {
                    server.SetStatus(value);
                }
            }
        }

        public bool TryGetServer(uint id, out ServerDetails server) => this.Servers.TryGetValue(id, out server);
        public IReadOnlyCollection<ServerDetails> GetServers() => (IReadOnlyCollection<ServerDetails>)this.Servers.Values;

        private void RedisServerStatusUpdate(RedisChannel channel, RedisValue value)
        {
            string[] data = value.ToString().Split('\0');
            if (data.Length == 2 && uint.TryParse(data[0], out uint serverId))
            {
                if (this.Servers.TryGetValue(serverId, out ServerDetails server))
                {
                    server.SetStatus(data[1]);
                }
                else //Okay so we have uncknown server, lets try load its date from sql, if we fail to load it then we just simply ignore this
                {
                    DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT id, name, ip, port FROM base.servers WHERE id = {serverId} LIMIT 1").ContinueWith(this.ParseSqlRedisFallback, data[1]));
                }
            }
        }

        private IReadOnlyCollection<ServerDetails> ParseSqlServers(Task<NpgsqlDataReader> task)
        {
            List<ServerDetails> servers = new();
            if (task.IsCompletedSuccessfully)
            {
                DbDataReader reader = task.Result;
                while (reader?.Read() ?? false)
                {
                    ServerDetails server = new(reader);

                    servers.Add(this.Servers.GetOrAdd(server.Id, new ServerDetails(reader)));
                }
            }
            else if (task.IsFaulted)
            {
                this.logger.LogError(EventIds.ServerLoadFailed, task.Exception, "Failed to load servers from sql");
            }

            return servers;
        }

        private void ParseSqlRedisFallback(Task<NpgsqlDataReader> task, object state)
        {
            if (task.IsCompletedSuccessfully)
            {
                DbDataReader reader = task.Result;
                if (reader?.Read() ?? false)
                {
                    ServerDetails server = new(reader);

                    this.Servers.GetOrAdd(server.Id, server).SetStatus((string)state);
                }
            }
            else if (task.IsFaulted)
            {
	            this.logger.LogError(EventIds.ServerLoadFailed, task.Exception, "Failed to load server from sql");
            }
        }
    }
}
