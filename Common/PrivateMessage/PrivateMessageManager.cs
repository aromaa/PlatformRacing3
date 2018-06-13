using log4net;
using Platform_Racing_3_Common.Database;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Platform_Racing_3_Common.PrivateMessage
{
    public static class PrivateMessageManager
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static Task<(uint Results, IReadOnlyList<IPrivateMessage> PMs)> GetUserPMsAsync(uint userId, uint start = 0, uint count = uint.MaxValue)
        {
            return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT COUNT(p.id) OVER() AS results, p.id, p.to_user_id, p.from_user_id, COALESCE(u.id, 0) AS from_user_id, COALESCE(u.username, 'Unknown') AS from_username, COALESCE(u.name_color, -14855017) AS from_name_color, p.type, p.sent_time, t.title, t.message, b.block_id, b.title AS block_title, l.level_id, l.title AS level_title FROM base.pms p LEFT JOIN base.users u ON u.id = p.from_user_id LEFT JOIN base.pms_text t ON t.id = p.id LEFT JOIN base.transfers_block b ON p.id = b.id LEFT JOIN base.transfers_level l ON p.id = l.id WHERE p.to_user_id = {userId} ORDER BY p.sent_time DESC OFFSET {start} LIMIT {count}").ContinueWith(PrivateMessageManager.ParseSqlMultiplePMs));
        }

        public static Task<IPrivateMessage> GetPrivateMessageAsync(uint id)
        {
            return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT COUNT(p.id) OVER() AS results, p.id, p.to_user_id, p.from_user_id, COALESCE(u.id, 0) AS from_user_id, COALESCE(u.username, 'Unknown') AS from_username, COALESCE(u.name_color, -14855017) AS from_name_color, p.type, p.sent_time, t.title, t.message, b.block_id, b.title AS block_title, l.level_id, l.title AS level_title FROM base.pms p LEFT JOIN base.users u ON u.id = p.from_user_id LEFT JOIN base.pms_text t ON t.id = p.id LEFT JOIN base.transfers_block b ON p.id = b.id LEFT JOIN base.transfers_level l ON p.id = l.id WHERE p.id = {id} LIMIT 1").ContinueWith(PrivateMessageManager.ParseSqlPm));
        }

        public static Task<IReadOnlyCollection<uint>> DeletePMsAsync(IReadOnlyCollection<uint> pms, uint? receiverUserId = default, uint? senderUserId = default)
        {
            if (receiverUserId == default && senderUserId == default)
            {
                return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"WITH deleted AS (DELETE FROM base.pms WHERE id IN({string.Join(',', pms):unsafe)}) RETURNING id, to_user_id, from_user_id, type, sent_time) INSERT INTO base.pms_deleted(id, to_user_id, from_user_id, type, sent_time) SELECT id, to_user_id, from_user_id, type, sent_time FROM deleted RETURNING id").ContinueWith(PrivateMessageManager.ParseSqlDeletePms));
            }
            else if (receiverUserId != default && senderUserId != default)
            {
                return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"WITH deleted AS (DELETE FROM base.pms WHERE to_user_id = {receiverUserId} AND from_user_id = {senderUserId} AND id IN({string.Join(',', pms):unsafe}) RETURNING id, to_user_id, from_user_id, type, sent_time) INSERT INTO base.pms_deleted(id, to_user_id, from_user_id, type, sent_time) SELECT id, to_user_id, from_user_id, type, sent_time FROM deleted RETURNING id").ContinueWith(PrivateMessageManager.ParseSqlDeletePms));
            }
            else if (receiverUserId != default)
            {
                return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"WITH deleted AS (DELETE FROM base.pms WHERE to_user_id = {receiverUserId} AND id IN({string.Join(',', pms):unsafe}) RETURNING id, to_user_id, from_user_id, type, sent_time) INSERT INTO base.pms_deleted(id, to_user_id, from_user_id, type, sent_time) SELECT id, to_user_id, from_user_id, type, sent_time FROM deleted RETURNING id").ContinueWith(PrivateMessageManager.ParseSqlDeletePms));
            }
            else
            {
                return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"WITH deleted AS (DELETE FROM base.pms WHERE from_user_id = {senderUserId} AND id IN({string.Join(',', pms):unsafe}) RETURNING id, to_user_id, from_user_id, type, sent_time) INSERT INTO base.pms_deleted(id, to_user_id, from_user_id, type, sent_time) SELECT id, to_user_id, from_user_id, type, sent_time FROM deleted RETURNING id").ContinueWith(PrivateMessageManager.ParseSqlDeletePms));
            }
        }

        public static Task<uint> SendTextPrivateMessageAsync(uint receiverUserId, uint senderUserId, string title, string message)
        {
            return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"WITH pm AS (INSERT INTO base.pms(to_user_id, from_user_id, type) VALUES({receiverUserId}, {senderUserId}, 'text') RETURNING id) INSERT INTO base.pms_text(id, title, message) SELECT id, {title}, {message} FROM pm RETURNING id").ContinueWith(PrivateMessageManager.ParseSqlSendTextPm));
        }

        public static Task ReportPrivateMessageAsync(uint receiverUserId, uint pmId)
        {
            return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ExecuteNonQueryAsync($"INSERT INTO base.pms_reported(id) SELECT id FROM base.pms WHERE id = {pmId} AND to_user_id = {receiverUserId} ON CONFLICT DO NOTHING"));
        }

        private static (uint Results, IReadOnlyList<IPrivateMessage> PMs) ParseSqlMultiplePMs(Task<DbDataReader> task)
        {
            uint results = 0;
            List<IPrivateMessage> pms = new List<IPrivateMessage>();
            if (task.IsCompletedSuccessfully)
            {
                DbDataReader reader = task.Result;
                while (reader?.Read() ?? false)
                {
                    if (results == 0)
                    {
                        results = (uint)(long)reader["results"];
                    }

                    pms.Add(PrivateMessageManager.ParseSqlPrivateMessage(reader));
                }
            }
            else if (task.IsFaulted)
            {
                PrivateMessageManager.Logger.Error($"Failed to load {nameof(IPrivateMessage)} from sql", task.Exception);
            }

            return (results, pms);
        }

        private static IPrivateMessage ParseSqlPm(Task<DbDataReader> task)
        {
            if (task.IsCompletedSuccessfully)
            {
                DbDataReader reader = task.Result;
                if (reader?.Read() ?? false)
                {
                    return PrivateMessageManager.ParseSqlPrivateMessage(reader);
                }
            }
            else if (task.IsFaulted)
            {
                PrivateMessageManager.Logger.Error($"Failed to load {nameof(IPrivateMessage)} from sql", task.Exception);
            }

            return null;
        }

        private static IPrivateMessage ParseSqlPrivateMessage(DbDataReader reader)
        {
            uint id = (uint)(int)reader["id"];
            string type = (string)reader["type"];
            DateTime sentTime = (DateTime)reader["sent_time"];
            if (type == "text" || type == "html")
            {
                return new TextPrivateMessage(id, (uint)(int)reader["to_user_id"], (uint)(int)reader["from_user_id"], (string)reader["from_username"], Color.FromArgb((int)reader["from_name_color"]), (string)reader["title"], (string)reader["message"], type == "html", sentTime);
            }
            else if (type == "block")
            {
                return new ThingTransferPrivateMessage(id, (uint)(int)reader["to_user_id"], (uint)(int)reader["from_user_id"], (string)reader["from_username"], Color.FromArgb((int)reader["from_name_color"]), "block", (string)reader["block_title"], (uint)(int)reader["block_id"], sentTime);
            }
            else if (type == "level")
            {
                return new ThingTransferPrivateMessage(id, (uint)(int)reader["to_user_id"], (uint)(int)reader["from_user_id"], (string)reader["from_username"], Color.FromArgb((int)reader["from_name_color"]), "level", (string)reader["level_title"], (uint)(int)reader["level_id"], sentTime);
            }
            else
            {
                throw new NotSupportedException($"Unsupported PM type {type}");
            }
        }

        private static IReadOnlyCollection<uint> ParseSqlDeletePms(Task<DbDataReader> task)
        {
            List<uint> pms = new List<uint>();
            if (task.IsCompletedSuccessfully)
            {
                DbDataReader reader = task.Result;
                if (reader?.Read() ?? false)
                {
                    pms.Add((uint)(int)reader["id"]);
                }
            }
            else if (task.IsFaulted)
            {
                PrivateMessageManager.Logger.Error($"Failed to parse deleted pms from sql", task.Exception);
            }

            return pms;
        }

        private static uint ParseSqlSendTextPm(Task<DbDataReader> task)
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
                PrivateMessageManager.Logger.Error($"Failed to insert text pm to sql", task.Exception);
            }

            return 0;
        }
    }
}
