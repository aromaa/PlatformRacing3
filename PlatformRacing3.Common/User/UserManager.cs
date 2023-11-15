﻿using System.Data.Common;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Npgsql;
using PlatformRacing3.Common.Customization;
using PlatformRacing3.Common.Database;
using PlatformRacing3.Common.Extensions;
using PlatformRacing3.Common.Redis;
using PlatformRacing3.Common.Utils;
using StackExchange.Redis;

namespace PlatformRacing3.Common.User;

public sealed class UserManager
{
	private static readonly ILogger<UserManager> logger = LoggerUtil.LoggerFactory.CreateLogger<UserManager>();

	//TODO: Player caching is kinda... ehhh... mess, we should have some kinda version number so we can decide what request should be thrown away
	//This is kinda complicated as the this class is supposed to be able to handle concurrency without issues also some kinda redis events should be implemented
	//This is not that important as we dont use cached results when we log in tho it has race conditions due to it not being perfect, this should not bring any big issues tho
	//Yeah, I'm lazy bastard at doing this properly at the moment

	private static readonly TimeSpan UserCacheTime = TimeSpan.FromDays(1);
	private static readonly MemoryCache Users = new(new MemoryCacheOptions()
	{
		ExpirationScanFrequency = TimeSpan.FromHours(1),
	});

	private static readonly TimeSpan UserIdsCacheTime = TimeSpan.FromDays(7);
	private static readonly MemoryCache UserIds = new(new MemoryCacheOptions()
	{
		ExpirationScanFrequency = TimeSpan.FromDays(1),
	});

	public static Task<PlayerUserData> TryGetUserDataByIdAsync(uint userId, bool allowCached = true)
	{
		if (userId == 0)
		{
			throw new ArgumentException(null, nameof(userId));
		}

		if (allowCached)
		{
			if (!UserManager.Users.TryGetValue(userId, out Lazy <PlayerUserData> lazyPlayerUserData)) //We are assuming that the user is already loaded into the memory thus not needing to do extra allocation for GetOrCreate method
			{
				lazyPlayerUserData = UserManager.Users.GetOrCreate(userId, (cacheEntry) =>
				{
					cacheEntry.SlidingExpiration = UserManager.UserCacheTime;

					return new Lazy<PlayerUserData>(() =>
					{
						return RedisConnection.GetDatabase().HashGetAsync($"users:{userId}", new RedisValue[]
						{
							"id",
							"username",

							"permission_rank",
							"name_color",
							"group_name",

							"last_login",
							"last_online",

							"total_exp",
							"bonus_exp",

							"hats",
							"heads",
							"bodys",
							"feets",

							"hat",
							"hatcolor",

							"head",
							"headcolor",

							"body",
							"bodycolor",

							"feet",
							"feetcolor",

							"speed",
							"accel",
							"jump",

							"campaigns",

							"friends",
							"ignored",
						}).ContinueWith(UserManager.ParseRedisUserData).ContinueWith((task) => task.Result ?? UserManager.TryGetUserDataByIdAsync(userId, false).Result).Result;
					}, LazyThreadSafetyMode.ExecutionAndPublication);
				});
			}

			if (lazyPlayerUserData.IsValueCreated)
			{
				return Task.FromResult(lazyPlayerUserData.Value);
			}

			return Task.Factory.StartNew(() => lazyPlayerUserData.Value);
		}

		return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT u.id, u.username, u.register_time, u.permission_rank, u.name_color, u.group_name, u.total_exp, u.bonus_exp, u.hats, u.heads, u.bodys, u.feets, u.current_hat, u.current_hat_color, u.current_head, u.current_head_color, u.current_body, u.current_body_color, u.current_feet, u.current_feet_color, u.speed, u.accel, u.jump, u.last_online, f.friends, i.ignored, c.campaign_runs, l.daily_luck FROM base.users u LEFT JOIN LATERAL (SELECT ARRAY_AGG(f.friend_user_id) AS friends FROM base.friends f WHERE f.user_id = u.id GROUP BY f.user_id) f ON TRUE LEFT JOIN LATERAL (SELECT array_agg(i.ignored_user_id) AS ignored FROM base.ignored i WHERE i.user_id = u.id) i ON TRUE LEFT JOIN LATERAL (SELECT array_agg((cr.level_id, cr.finish_time)) AS campaign_runs FROM base.campaigns_runs cr WHERE cr.user_id = u.id) c ON TRUE LEFT JOIN LATERAL (SELECT array_agg((udl.date, udl.uses)) AS daily_luck FROM base.users_daily_luck udl WHERE udl.user_id = u.id) l ON TRUE WHERE u.id = {userId} LIMIT 1").ContinueWith(UserManager.ParseSqlUserData));
	}

	public static Task<PlayerUserData> TryGetUserDataByNameAsync(string username, bool allowCached = true)
	{
		if (username == null)
		{
			throw new ArgumentException(null, nameof(username));
		}

		username = username.ToUpperInvariant(); //Hmh.... Case insetivity needed here
            
		if (allowCached)
		{
			if (!UserManager.UserIds.TryGetValue(username, out Lazy<uint> lazyUserId))
			{
				lazyUserId = UserManager.UserIds.GetOrCreate(username, (cacheEntry) =>
				{
					cacheEntry.SlidingExpiration = UserManager.UserIdsCacheTime;

					return new Lazy<uint>(() =>
					{
						RedisValue userId = RedisConnection.GetDatabase().HashGet($"userids:{Math.Floor(username.CountCharsSum() / 100.0)}", username);
						if (userId.HasValue)
						{
							return (uint)userId;
						}
						else
						{
							return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT u.id, u.username, u.register_time, u.permission_rank, u.name_color, u.group_name, u.total_exp, u.bonus_exp, u.hats, u.heads, u.bodys, u.feets, u.current_hat, u.current_hat_color, u.current_head, u.current_head_color, u.current_body, u.current_body_color, u.current_feet, u.current_feet_color, u.speed, u.accel, u.jump, u.last_online, f.friends, i.ignored, c.campaign_runs, l.daily_luck FROM base.users u LEFT JOIN LATERAL (SELECT ARRAY_AGG(f.friend_user_id) AS friends FROM base.friends f WHERE f.user_id = u.id GROUP BY f.user_id) f ON TRUE LEFT JOIN LATERAL (SELECT array_agg(i.ignored_user_id) AS ignored FROM base.ignored i WHERE i.user_id = u.id) i ON TRUE LEFT JOIN LATERAL (SELECT array_agg((cr.level_id, cr.finish_time)) AS campaign_runs FROM base.campaigns_runs cr WHERE cr.user_id = u.id) c ON TRUE LEFT JOIN LATERAL (SELECT array_agg((udl.date, udl.uses)) AS daily_luck FROM base.users_daily_luck udl WHERE udl.user_id = u.id) l ON TRUE WHERE u.username ILIKE {username} LIMIT 1").ContinueWith(UserManager.ParseSqlUserData)).Result?.Id ?? 0;
						}
					}, LazyThreadSafetyMode.ExecutionAndPublication);
				});
			}

			if (lazyUserId.IsValueCreated)
			{
				uint userId = lazyUserId.Value;
				if (userId > 0)
				{
					return UserManager.TryGetUserDataByIdAsync(userId);
				}
				else
				{
					return Task.FromResult<PlayerUserData>(null);
				}
			}

			return Task.Factory.StartNew(() =>
			{
				uint userId = lazyUserId.Value;
				if (userId > 0)
				{
					return UserManager.TryGetUserDataByIdAsync(userId);
				}
				else
				{
					return Task.FromResult<PlayerUserData>(null);
				}
			}).Unwrap(); //Unwrap should be fine?
		}

		return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT u.id, u.username, u.register_time, u.permission_rank, u.name_color, u.group_name, u.total_exp, u.bonus_exp, u.hats, u.heads, u.bodys, u.feets, u.current_hat, u.current_hat_color, u.current_head, u.current_head_color, u.current_body, u.current_body_color, u.current_feet, u.current_feet_color, u.speed, u.accel, u.jump, u.last_online, f.friends, i.ignored, c.campaign_runs, l.daily_luck FROM base.users u LEFT JOIN LATERAL (SELECT ARRAY_AGG(f.friend_user_id) AS friends FROM base.friends f WHERE f.user_id = u.id GROUP BY f.user_id) f ON TRUE LEFT JOIN LATERAL (SELECT array_agg(i.ignored_user_id) AS ignored FROM base.ignored i WHERE i.user_id = u.id) i ON TRUE LEFT JOIN LATERAL (SELECT array_agg((cr.level_id, cr.finish_time)) AS campaign_runs FROM base.campaigns_runs cr WHERE cr.user_id = u.id) c ON TRUE LEFT JOIN LATERAL (SELECT array_agg((udl.date, udl.uses)) AS daily_luck FROM base.users_daily_luck udl WHERE udl.user_id = u.id) l ON TRUE WHERE u.username ILIKE {username} LIMIT 1").ContinueWith(UserManager.ParseSqlUserData));
	}

	public static Task<PlayerUserData> TryGetUserDataByEmailAsync(string email)
	{
		if (email == null)
		{
			throw new ArgumentException(null, nameof(email));
		}

		return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT u.id, u.username, u.register_time, u.permission_rank, u.name_color, u.group_name, u.total_exp, u.bonus_exp, u.hats, u.heads, u.bodys, u.feets, u.current_hat, u.current_hat_color, u.current_head, u.current_head_color, u.current_body, u.current_body_color, u.current_feet, u.current_feet_color, u.speed, u.accel, u.jump, u.last_online, f.friends, i.ignored, c.campaign_runs, l.daily_luck FROM base.users u LEFT JOIN LATERAL (SELECT ARRAY_AGG(f.friend_user_id) AS friends FROM base.friends f WHERE f.user_id = u.id GROUP BY f.user_id) f ON TRUE LEFT JOIN LATERAL (SELECT array_agg(i.ignored_user_id) AS ignored FROM base.ignored i WHERE i.user_id = u.id) i ON TRUE LEFT JOIN LATERAL (SELECT array_agg((cr.level_id, cr.finish_time)) AS campaign_runs FROM base.campaigns_runs cr WHERE cr.user_id = u.id) c ON TRUE LEFT JOIN LATERAL (SELECT array_agg((udl.date, udl.uses)) AS daily_luck FROM base.users_daily_luck udl WHERE udl.user_id = u.id) l ON TRUE WHERE u.email ILIKE {email} LIMIT 1").ContinueWith(UserManager.ParseSqlUserData));
	}

	public static Task<PlayerUserData> TryCreateNewUserAsync(string username, string password, string email, IPAddress ip) => DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"INSERT INTO base.users(username, password, email, register_ip) VALUES({username}, {PasswordUtils.HashPassword(password)}, {email}, {ip}) RETURNING id, username, register_time, permission_rank, name_color, group_name, total_exp, bonus_exp, hats, heads, bodys, feets, current_hat, current_hat_color, current_head, current_head_color, current_body, current_body_color, current_feet, current_feet_color, speed, accel, jump, last_online, NULL AS friends, NULL AS ignored, NULL AS campaign_runs, NULL AS daily_luck").ContinueWith(UserManager.ParseSqlUserData));

	public static Task<uint> TryAuthenicateAsync(string identifier, string password)
	{
		Task<NpgsqlDataReader> userDataTask = null;
		DatabaseConnection dbConnection = new();

		try
		{
			try
			{
				new MailAddress(identifier); //TODO: Alternativly we could use regex

				userDataTask = dbConnection.ReadDataAsync($"SELECT id, password FROM base.users WHERE email ILIKE {identifier} LIMIT 1");
			}
			catch
			{
				userDataTask = dbConnection.ReadDataAsync($"SELECT id, password FROM base.users WHERE username ILIKE {identifier} LIMIT 1");
			}

			Task<uint> resultTask = userDataTask.ContinueWith((task) =>
			{
				if (task.IsCompletedSuccessfully)
				{
					DbDataReader reader = task.Result;
					if (reader?.Read() ?? false)
					{
						uint userId = (uint)(int)reader["id"];
						string dbPassword = (string)reader["password"];
						if (PasswordUtils.VerifyPassword(password, dbPassword))
						{
							return userId;
						}
#pragma warning disable CS0618 // We know what we are doing, tryna be good guys
						else if (PasswordUtils.VerifyPasswordLegacy(password, dbPassword))
#pragma warning restore CS0618
						{
							//ALERT!!! LEGACY PASSWORD FOUND!!!!! UPDATE PASSWORD!!!!!!!
							DatabaseConnection.NewAsyncConnection((dbConnection_) => dbConnection_.ExecuteNonQueryAsync($"UPDATE base.users SET password = {PasswordUtils.HashPassword(password)} WHERE id = {userId}"));

							return userId;
						}
					}
				}
				else if (task.IsFaulted)
				{
					UserManager.logger.LogError(EventIds.UserAuthenticationFailed, task.Exception, "Failed to authenicate user");
				}

				return 0u;
			});

			Task.WhenAll(resultTask).ContinueWith((task) => dbConnection.Dispose());

			return resultTask;
		}
		catch
		{
			dbConnection.Dispose();
		}

		return Task.FromResult(0u);
	}

	public static Task UpdateStatsAsync(uint userId, uint speed, uint accel, uint jump)
	{
		//Update to redis
		return RedisConnection.GetDatabase().HashSetAsync($"users:{userId}", new HashEntry[]
		{
			new HashEntry("speed", speed),
			new HashEntry("accel", accel),
			new HashEntry("jump", jump),
		}, CommandFlags.FireAndForget);
	}

	public static Task SetPartsAsync(uint userId, Hat hat, Color hatColor, Part head, Color headColor, Part body, Color bodyColor, Part feet, Color feetColor)
	{
		//Update to redis
		return RedisConnection.GetDatabase().HashSetAsync($"users:{userId}", new HashEntry[]
		{
			new HashEntry("hat", (uint)hat),
			new HashEntry("hatcolor", hatColor.ToArgb()),

			new HashEntry("head", (uint)head),
			new HashEntry("headcolor", headColor.ToArgb()),

			new HashEntry("body", (uint)body),
			new HashEntry("bodycolor", bodyColor.ToArgb()),

			new HashEntry("feet", (uint)feet),
			new HashEntry("feetcolor", feetColor.ToArgb()),
		}, CommandFlags.FireAndForget);
	}

	//Update to sql and then call SetTotalExp on PlayerUserData and then update to redis
	public static Task AddExpAsync(uint userId, ulong addExp, ulong totalExp, uint rank, ulong exp) => DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"UPDATE base.users SET total_exp = total_exp + {addExp}, rank = {rank}, exp = {exp} WHERE id = {userId} RETURNING id, total_exp").ContinueWith(UserManager.ParseSqlAddExp));

	//Update to sql and then call SetBonusExp on PlayerUserData and then update to redis
	public static Task GiveBonusExp(uint userId, ulong amount) => DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"UPDATE base.users SET bonus_exp = bonus_exp + {amount} WHERE id = {userId} RETURNING id, bonus_exp").ContinueWith(UserManager.ParseSqlBonusExpUpdate));
	public static Task DrainBonusExp(uint userId, ulong drainBonusExp) => DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"UPDATE base.users SET bonus_exp = bonus_exp - {drainBonusExp} WHERE id = {userId} RETURNING id, bonus_exp").ContinueWith(UserManager.ParseSqlBonusExpUpdate));

	public static Task AddFriendAsync(uint userId, uint friendId) => DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ExecuteNonQueryAsync($"INSERT INTO base.friends(user_id, friend_user_id) VALUES({userId}, {friendId}) ON CONFLICT DO NOTHING"));
	public static Task RemoveFriendAsync(uint userId, uint ignoredId) => DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ExecuteNonQueryAsync($"DELETE FROM base.friends WHERE user_id = {userId} AND friend_user_id = {ignoredId}"));

	public static Task<uint> CountMyFriendsAsync(uint userId) =>  DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT COUNT(user_id) AS friends_count FROM base.friends WHERE user_id = {userId}").ContinueWith(UserManager.ParseSqlMyFriendsCount));
	public static Task<IReadOnlyCollection<PlayerUserData>> GetMyFriendsAsync(uint userId, uint start = 0, uint count = uint.MaxValue) => DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT u.id, u.username, u.register_time, u.permission_rank, u.name_color, u.group_name, u.total_exp, u.bonus_exp, u.hats, u.heads, u.bodys, u.feets, u.current_hat, u.current_hat_color, u.current_head, u.current_head_color, u.current_body, u.current_body_color, u.current_feet, u.current_feet_color, u.speed, u.accel, u.jump, u.last_online, f.friends, i.ignored, c.campaign_runs, l.daily_luck FROM base.friends ff LEFT JOIN base.users u ON u.id = ff.friend_user_id LEFT JOIN LATERAL (SELECT ARRAY_AGG(f.friend_user_id) AS friends FROM base.friends f WHERE f.user_id = u.id GROUP BY f.user_id) f ON TRUE LEFT JOIN LATERAL (SELECT array_agg(i.ignored_user_id) AS ignored FROM base.ignored i WHERE i.user_id = u.id) i ON TRUE LEFT JOIN LATERAL (SELECT array_agg((cr.level_id, cr.finish_time)) AS campaign_runs FROM base.campaigns_runs cr WHERE cr.user_id = u.id) c ON TRUE LEFT JOIN LATERAL (SELECT array_agg((udl.date, udl.uses)) AS daily_luck FROM base.users_daily_luck udl WHERE udl.user_id = u.id) l ON TRUE WHERE ff.user_id = {userId} OFFSET {start} LIMIT {count}").ContinueWith(UserManager.ParseSqlMultipleUserData));

	public static Task AddIgnoredAsync(uint userId, uint friendId) => DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ExecuteNonQueryAsync($"INSERT INTO base.ignored(user_id, ignored_user_id) VALUES({userId}, {friendId}) ON CONFLICT DO NOTHING"));
	public static Task RemoveIgnoredAsync(uint userId, uint ignoredId) => DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ExecuteNonQueryAsync($"DELETE FROM base.ignored WHERE user_id = {userId} AND ignored_user_id = {ignoredId}"));

	public static Task<uint> CountMyIgnoredAsync(uint userId) => DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT COUNT(user_id) AS ignored_count FROM base.ignored WHERE user_id = {userId}").ContinueWith(UserManager.ParseSqlMyIgnoredCount));
	public static Task<IReadOnlyCollection<PlayerUserData>> GetMyIgnoredAsync(uint userId, uint start = 0, uint count = uint.MaxValue) =>  DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT u.id, u.username, u.register_time, u.permission_rank, u.name_color, u.group_name, u.total_exp, u.bonus_exp, u.hats, u.heads, u.bodys, u.feets, u.current_hat, u.current_hat_color, u.current_head, u.current_head_color, u.current_body, u.current_body_color, u.current_feet, u.current_feet_color, u.speed, u.accel, u.jump, u.last_online, f.friends, i.ignored, c.campaign_runs, l.daily_luck FROM base.ignored ii LEFT JOIN base.users u ON u.id = ii.ignored_user_id LEFT JOIN LATERAL (SELECT ARRAY_AGG(f.friend_user_id) AS friends FROM base.friends f WHERE f.user_id = u.id GROUP BY f.user_id) f ON TRUE LEFT JOIN LATERAL (SELECT array_agg(i.ignored_user_id) AS ignored FROM base.ignored i WHERE i.user_id = u.id) i ON TRUE LEFT JOIN LATERAL (SELECT array_agg((cr.level_id, cr.finish_time)) AS campaign_runs FROM base.campaigns_runs cr WHERE cr.user_id = u.id) c ON TRUE LEFT JOIN LATERAL (SELECT array_agg((udl.date, udl.uses)) AS daily_luck FROM base.users_daily_luck udl WHERE udl.user_id = u.id) l ON TRUE WHERE ii.user_id = {userId} OFFSET {start} LIMIT {count}").ContinueWith(UserManager.ParseSqlMultipleUserData));

	public static Task<IReadOnlyCollection<PlayerUserData>> SearchUsers(string name) => DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT u.id, u.username, u.register_time, u.permission_rank, u.name_color, u.group_name, u.total_exp, u.bonus_exp, u.hats, u.heads, u.bodys, u.feets, u.current_hat, u.current_hat_color, u.current_head, u.current_head_color, u.current_body, u.current_body_color, u.current_feet, u.current_feet_color, u.speed, u.accel, u.jump, u.last_online, f.friends, i.ignored, c.campaign_runs, l.daily_luck FROM base.users u LEFT JOIN LATERAL (SELECT ARRAY_AGG(f.friend_user_id) AS friends FROM base.friends f WHERE f.user_id = u.id GROUP BY f.user_id) f ON TRUE LEFT JOIN LATERAL (SELECT array_agg(i.ignored_user_id) AS ignored FROM base.ignored i WHERE i.user_id = u.id) i ON TRUE LEFT JOIN LATERAL (SELECT array_agg((cr.level_id, cr.finish_time)) AS campaign_runs FROM base.campaigns_runs cr WHERE cr.user_id = u.id) c ON TRUE LEFT JOIN LATERAL (SELECT array_agg((udl.date, udl.uses)) AS daily_luck FROM base.users_daily_luck udl WHERE udl.user_id = u.id) l ON TRUE WHERE u.username ILIKE '%' || {name} || '%'").ContinueWith(UserManager.ParseSqlMultipleUserData));

	public static Task<IReadOnlyDictionary<uint, int>> GetCampaignRuns(uint userId) => DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT array_agg(ARRAY[level_id, finish_time]) AS campaign_runs FROM base.campaigns_runs WHERE user_id = {userId}").ContinueWith(UserManager.ParseSqlCampaignRuns));

	public static Task GiveHat(uint userId, Hat hat) => DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"UPDATE base.users SET hats = base.UNIQ(base.SORT(ARRAY_APPEND(hats, {(uint)hat}::int))) WHERE id = {userId} RETURNING id, hats").ContinueWith(UserManager.ParseSqlAddHat));
	public static Task GiveHead(uint userId, Part part) => DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"UPDATE base.users SET heads = base.UNIQ(base.SORT(ARRAY_APPEND(heads, {(uint)part}::int))) WHERE id = {userId} RETURNING id, heads").ContinueWith(UserManager.ParseSqlAddHead));
	public static Task GiveBody(uint userId, Part part) => DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"UPDATE base.users SET bodys = base.UNIQ(base.SORT(ARRAY_APPEND(bodys, {(uint)part}::int))) WHERE id = {userId} RETURNING id, bodys").ContinueWith(UserManager.ParseSqlAddBody));
	public static Task GiveFeet(uint userId, Part part) => DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"UPDATE base.users SET feets = base.UNIQ(base.SORT(ARRAY_APPEND(feets, {(uint)part}::int))) WHERE id = {userId} RETURNING id, feets").ContinueWith(UserManager.ParseSqlAddFeet));
	public static Task GiveSet(uint userId, Part part) => Task.WhenAll(UserManager.GiveHead(userId, part), UserManager.GiveBody(userId, part), UserManager.GiveFeet(userId, part)); //TODO: More optimized version

	public static Task<bool> TryCreateForgotPasswordToken(uint userId, string token, IPAddress ip) => DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"INSERT INTO base.users_password_reset(token, user_id, user_ip) VALUES({token}, {userId}, {ip}) RETURNING true").ContinueWith(UserManager.ParseSqlInsertPasswordReset));

	public static Task<bool> CanSendPasswordToken(uint userId) => DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT COUNT(*) AS count FROM base.users_password_reset WHERE user_id = {userId} AND requested >= NOW() - '30 minutes'::interval").ContinueWith(UserManager.ParseSqlCanSendPasswordToken));

	public static Task<uint> TryGetForgotPasswordToken(string token) => DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT user_id FROM base.users_password_reset WHERE token = {token} AND used IS NULL AND requested >= NOW() - '2 days'::interval").ContinueWith(UserManager.ParseSqlGetPasswordToken));

	public static Task<bool> TryConsumePasswordToken(string token, string password, IPAddress ip) => DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"WITH updateToken AS(UPDATE base.users_password_reset SET used = NOW(), used_ip = {ip} WHERE token = {token} AND used IS NULL AND requested >= NOW() - '2 days'::interval RETURNING user_id) UPDATE base.users SET password = {PasswordUtils.HashPassword(password)} WHERE id = (SELECT user_id FROM updateToken) RETURNING true").ContinueWith(UserManager.ParseSqlConsumePasswordToken));

	public static Task ConsumeLuck(params uint[] userIds) => DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"INSERT INTO base.users_daily_luck(user_id, uses) SELECT users, 1 FROM UNNEST({userIds}) users ON CONFLICT(user_id, date) DO UPDATE SET uses = users_daily_luck.uses + excluded.uses RETURNING user_id, date, uses").ContinueWith(UserManager.ParseSqlDrainLuck));

	public static Task<bool> TryInsertDiscordLinkage(uint userId, ulong discordId) => DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"INSERT INTO base.users_discord_link(user_id, discord_id) VALUES({userId}, {discordId}) RETURNING true").ContinueWith(UserManager.ParseSqlInsertDiscordLink));

	public static Task<(uint UserId, ulong DiscordId)> HasDiscordLinkage(uint userId, ulong discordId) => DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT user_id, discord_id FROM base.users_discord_link WHERE user_id = {userId} OR discord_id = {discordId} LIMIT 1").ContinueWith(UserManager.ParseSqlGetDiscordLink2));

	public static Task<uint> HasDiscordLinkage(ulong discordId) => DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"SELECT user_id FROM base.users_discord_link WHERE discord_id = {discordId} LIMIT 1").ContinueWith(UserManager.ParseSqlGetDiscordLink));

	public static Task<bool> UseCoins(uint userId, uint price)
	{
		return DatabaseConnection.NewAsyncConnection(c =>
			c.ExecuteNonQueryAsync($"UPDATE base.users SET coins = coins - {(int)price} WHERE id = {userId} AND coins >= {(int)price}").ContinueWith(Parse));

		static bool Parse(Task<int> task)
		{
			if (task.IsCompletedSuccessfully)
			{
				return task.Result == 1;
			}
			else if (task.IsFaulted)
			{
				UserManager.logger.LogError(EventIds.UserLoadFailed, task.Exception, $"Failed to complete item purchase to sql");
			}

			return false;
		}
	}
	
	public static Task<bool> PurchaseLevelItem(uint userId, uint levelId, string itemId, uint price, uint receiveAmount)
	{
		return DatabaseConnection.NewAsyncConnection(c =>
			c.ReadDataAsync($"CALL base.purchase_item({(int)userId}, {(int)levelId}, {itemId}, {(int)price}, {(int)receiveAmount}, -1)").ContinueWith(Parse));

		static bool Parse(Task<NpgsqlDataReader> task)
		{
			if (task.IsCompletedSuccessfully)
			{
				NpgsqlDataReader reader = task.Result;
				if (reader.Read())
				{
					return (int)reader["status"] == 1;
				}
			}
			else if (task.IsFaulted)
			{
				UserManager.logger.LogError(EventIds.UserLoadFailed, task.Exception, $"Failed to complete item purchase to sql");
			}

			return false;
		}
	}

	public static Task<List<string>> GetPurchasedItems(uint userId, uint levelId)
	{
		return DatabaseConnection.NewAsyncConnection(c =>
			c.ReadDataAsync($"SELECT item_id FROM base.user_level_purchases WHERE user_id = {userId} AND level_id = {levelId}").ContinueWith(Parse));

		static List<string> Parse(Task<NpgsqlDataReader> task)
		{
			List<string> items = new();
			if (task.IsCompletedSuccessfully)
			{
				NpgsqlDataReader reader = task.Result;
				while (reader.Read())
				{
					items.Add((string)reader["item_id"]);
				}
			}
			else if (task.IsFaulted)
			{
				UserManager.logger.LogError(EventIds.UserLoadFailed, task.Exception, $"Failed to load purchases from sql");
			}

			return items;
		}
	}

	private static PlayerUserData ParseRedisUserData(Task<RedisValue[]> task)
	{
		if (task.IsCompletedSuccessfully)
		{
			RedisValue[] results = task.Result;
			if (results?.All(v => v.HasValue) ?? false)
			{
				PlayerUserData playerUserData = new(results);
				return UserManager.CachePlayerUserData(playerUserData, true);
			}
		}
		else if (task.IsFaulted)
		{
			UserManager.logger.LogError(EventIds.UserLoadFailed, task.Exception, $"Failed to load {nameof(PlayerUserData)} from redis");
		}

		return null;
	}

	private static PlayerUserData ParseSqlUserData(Task<NpgsqlDataReader> task)
	{
		if (task.IsCompletedSuccessfully)
		{
			DbDataReader reader = task.Result;
			if (reader?.Read() ?? false)
			{
				PlayerUserData playerUserData = new(reader);
				return UserManager.CachePlayerUserData(playerUserData);
			}
		}
		else if (task.IsFaulted)
		{
			UserManager.logger.LogError(EventIds.UserLoadFailed, task.Exception, $"Failed to load {nameof(PlayerUserData)} from sql");
		}

		return null;
	}

	private static PlayerUserData CachePlayerUserData(PlayerUserData playerUserData, bool loadedFromRedis = false)
	{
		//Push it to redis to keep it up to date
		if (!loadedFromRedis) //We loaded it from redis... there is no point to push data back to there
		{
			RedisConnection.GetDatabase().HashSetAsync($"users:{playerUserData.Id}", playerUserData.ToRedis(), CommandFlags.FireAndForget);
		}

		//Make sure to keep the usernames sync
		RedisConnection.GetDatabase().HashSetAsync($"userids:{Math.Floor(playerUserData.Username.ToUpperInvariant().CountCharsSum() / 100.0)}", new HashEntry[] //TODO: Umh? Also ToUpper due to case sensetivity
		{
			new HashEntry(playerUserData.Username.ToUpperInvariant(), playerUserData.Id)
		}, CommandFlags.FireAndForget);

		UserManager.UserIds.GetOrCreate(playerUserData.Username.ToUpperInvariant(), (cacheEntry) =>
		{
			cacheEntry.SlidingExpiration = UserManager.UserIdsCacheTime;

			return new Lazy<uint>(playerUserData.Id);
		});

		Lazy<PlayerUserData> lazy = UserManager.Users.GetOrCreate(playerUserData.Id, (cacheEntry) =>
		{
			cacheEntry.SlidingExpiration = UserManager.UserCacheTime;

			return new Lazy<PlayerUserData>(playerUserData);
		});

		if (lazy.IsValueCreated)
		{
			lazy.Value.Merge(playerUserData);
			return lazy.Value;
		}
		else
		{
			return playerUserData;
		}
	}

	private static IReadOnlyCollection<PlayerUserData> ParseSqlMultipleUserData(Task<NpgsqlDataReader> task)
	{
		List<PlayerUserData> users = new();
		if (task.IsCompletedSuccessfully)
		{
			DbDataReader reader = task.Result;
			while (reader?.Read() ?? false)
			{
				PlayerUserData playerUserData = new(reader);
				users.Add(UserManager.CachePlayerUserData(playerUserData));
			}
		}
		else if (task.IsFaulted)
		{
			UserManager.logger.LogError(EventIds.UserLoadFailed, task.Exception, $"Failed to load multiple {nameof(PlayerUserData)} from sql");
		}

		return users;
	}

	private static IReadOnlyDictionary<uint, int> ParseSqlCampaignRuns(Task<NpgsqlDataReader> task)
	{
		IDictionary<uint, int> runs = new Dictionary<uint, int>();

		if (task.IsCompletedSuccessfully)
		{
			DbDataReader reader = task.Result;
			while (reader?.Read() ?? false)
			{
				object campaignRuns = reader["campaign_runs"]; //Might be null
				if (campaignRuns is int[,] campaignRuns_)
				{
					for (int i = 0; i < campaignRuns_.Length / campaignRuns_.Rank; i++)
					{
						uint levelId = (uint)campaignRuns_[i, 0];
						if (levelId > 0)
						{
							int finishTime = campaignRuns_[i, 1];

							runs[levelId] = finishTime;
						}
					}
				}
			}
		}
		else if (task.IsFaulted)
		{
			UserManager.logger.LogError(EventIds.UserCampaignDataLoadFailed, task.Exception, "Failed to load campaign runs from sql");
		}

		return (IReadOnlyDictionary<uint, int>)runs;
	}

	private static uint ParseSqlMyFriendsCount(Task<NpgsqlDataReader> task)
	{
		if (task.IsCompletedSuccessfully)
		{
			DbDataReader reader = task.Result;
			if (reader?.Read() ?? false)
			{
				return (uint)(long)reader["friends_count"];
			}
		}
		else if (task.IsFaulted)
		{
			UserManager.logger.LogError(EventIds.UserFriendsLoadFailed, task.Exception, "Failed to load my friends count from sql");
		}

		return 0;
	}

	private static uint ParseSqlMyIgnoredCount(Task<NpgsqlDataReader> task)
	{
		if (task.IsCompletedSuccessfully)
		{
			DbDataReader reader = task.Result;
			if (reader?.Read() ?? false)
			{
				return (uint)(long)reader["ignored_count"];
			}
		}
		else if (task.IsFaulted)
		{
			UserManager.logger.LogError(EventIds.UserIgnoredLoadFailed, task.Exception, "Failed to load my ignored count from sql");
		}

		return 0;
	}

	private static void ParseSqlAddExp(Task<NpgsqlDataReader> task)
	{
		if (task.IsCompletedSuccessfully)
		{
			DbDataReader reader = task.Result;
			if (reader?.Read() ?? false)
			{
				uint userId = (uint)(int)reader["id"];
				ulong totalExp = (ulong)(long)reader["total_exp"];

				UserManager.TryGetUserDataByIdAsync(userId).Result?.SetTotalExp(totalExp);
			}
		}
		else if (task.IsFaulted)
		{
			UserManager.logger.LogError(EventIds.UserUpdateFailed, task.Exception, "Failed to update total exp to sql");
		}
	}

	private static void ParseSqlBonusExpUpdate(Task<NpgsqlDataReader> task)
	{
		if (task.IsCompletedSuccessfully)
		{
			DbDataReader reader = task.Result;
			if (reader?.Read() ?? false)
			{
				uint userId = (uint)(int)reader["id"];
				ulong bonusExp = (ulong)(long)reader["bonus_exp"];

				UserManager.TryGetUserDataByIdAsync(userId).Result?.SetTotalBonusExp(bonusExp);
			}
		}
		else if (task.IsFaulted)
		{
			UserManager.logger.LogError(EventIds.UserUpdateFailed, task.Exception, "Failed to update total exp to sql");
		}
	}

	private static void ParseSqlAddHat(Task<NpgsqlDataReader> task)
	{
		if (task.IsCompletedSuccessfully)
		{
			DbDataReader reader = task.Result;
			if (reader?.Read() ?? false)
			{
				uint userId = (uint)(int)reader["id"];
				IEnumerable<Hat> hats = ((int[])reader["hats"]).Select((h) => (Hat)h);

				UserManager.TryGetUserDataByIdAsync(userId).Result?.SetHats(hats);
			}
		}
		else if (task.IsFaulted)
		{
			UserManager.logger.LogError(EventIds.UserUpdateFailed, task.Exception, "Failed to add user hat to sql");
		}
	}

	private static void ParseSqlAddHead(Task<NpgsqlDataReader> task)
	{
		if (task.IsCompletedSuccessfully)
		{
			DbDataReader reader = task.Result;
			if (reader?.Read() ?? false)
			{
				uint userId = (uint)(int)reader["id"];
				IEnumerable<Part> heads = ((int[])reader["heads"]).Select((h) => (Part)h);

				UserManager.TryGetUserDataByIdAsync(userId).Result?.SetHeads(heads);
			}
		}
		else if (task.IsFaulted)
		{
			UserManager.logger.LogError(EventIds.UserUpdateFailed, task.Exception, "Failed to add user head to sql");
		}
	}

	private static void ParseSqlAddBody(Task<NpgsqlDataReader> task)
	{
		if (task.IsCompletedSuccessfully)
		{
			DbDataReader reader = task.Result;
			if (reader?.Read() ?? false)
			{
				uint userId = (uint)(int)reader["id"];
				IEnumerable<Part> bodys = ((int[])reader["bodys"]).Select((h) => (Part)h);

				UserManager.TryGetUserDataByIdAsync(userId).Result?.SetBodys(bodys);
			}
		}
		else if (task.IsFaulted)
		{
			UserManager.logger.LogError(EventIds.UserUpdateFailed, task.Exception, "Failed to add user body to sql");
		}
	}

	private static void ParseSqlAddFeet(Task<NpgsqlDataReader> task)
	{
		if (task.IsCompletedSuccessfully)
		{
			DbDataReader reader = task.Result;
			if (reader?.Read() ?? false)
			{
				uint userId = (uint)(int)reader["id"];
				IEnumerable<Part> feets = ((int[])reader["feets"]).Select((h) => (Part)h);

				UserManager.TryGetUserDataByIdAsync(userId).Result?.SetFeets(feets);
			}
		}
		else if (task.IsFaulted)
		{
			UserManager.logger.LogError(EventIds.UserUpdateFailed, task.Exception, "Failed to add user feet to sql");
		}
	}
        
	private static bool ParseSqlInsertPasswordReset(Task<NpgsqlDataReader> task)
	{
		if (task.IsCompletedSuccessfully)
		{
			DbDataReader reader = task.Result;
			if (reader?.Read() ?? false)
			{
				return true;
			}
		}
		else if (task.IsFaulted)
		{
			UserManager.logger.LogError(EventIds.UserUpdateFailed, task.Exception, "Failed to insert password reset token to sql");
		}

		return false;
	}

	private static bool ParseSqlCanSendPasswordToken(Task<NpgsqlDataReader> task)
	{
		if (task.IsCompletedSuccessfully)
		{
			DbDataReader reader = task.Result;
			if (reader?.Read() ?? false)
			{
				long count = (long)reader["count"];

				return count == 0;
			}
		}
		else if (task.IsFaulted)
		{
			UserManager.logger.LogError(EventIds.UserUpdateFailed, task.Exception, "Failed to check if password reset token can be send again from sql");

			return false;
		}

		return true;
	}

	private static uint ParseSqlGetPasswordToken(Task<NpgsqlDataReader> task)
	{
		if (task.IsCompletedSuccessfully)
		{
			DbDataReader reader = task.Result;
			if (reader?.Read() ?? false)
			{
				uint userId = (uint)(int)reader["user_id"];

				return userId;
			}
		}
		else if (task.IsFaulted)
		{
			UserManager.logger.LogError(EventIds.UserUpdateFailed, task.Exception, "Failed to get password token from sql");
		}

		return 0;
	}

	private static bool ParseSqlConsumePasswordToken(Task<NpgsqlDataReader> task)
	{
		if (task.IsCompletedSuccessfully)
		{
			DbDataReader reader = task.Result;
			if (reader?.Read() ?? false)
			{
				return true;
			}
		}
		else if (task.IsFaulted)
		{
			UserManager.logger.LogError(EventIds.UserUpdateFailed, task.Exception, "Failed to consume password reset token from sql");
		}

		return false;
	}

	private static void ParseSqlDrainLuck(Task<NpgsqlDataReader> task)
	{
		if (task.IsCompletedSuccessfully)
		{
			DbDataReader reader = task.Result;
			while (reader?.Read() ?? false)
			{
				uint userId = (uint)(int)reader["user_id"];
				DateTime date = (DateTime)reader["date"];
				uint consumed = (uint)(int)reader["uses"];

				UserManager.TryGetUserDataByIdAsync(userId).Result?.SetDailyLuckConsumed(date, consumed);
			}
		}
		else if (task.IsFaulted)
		{
			UserManager.logger.LogError(EventIds.UserUpdateFailed, task.Exception, "Failed to drawin luck from sql");
		}
	}

	private static bool ParseSqlInsertDiscordLink(Task<NpgsqlDataReader> task)
	{
		if (task.IsCompletedSuccessfully)
		{
			DbDataReader reader = task.Result;
			while (reader?.Read() ?? false)
			{
				return true;
			}
		}
		else if (task.IsFaulted)
		{
			UserManager.logger.LogError(EventIds.UserUpdateFailed, task.Exception, "Failed to insert discord linkage to sql");
		}

		return false;
	}

	private static uint ParseSqlGetDiscordLink(Task<NpgsqlDataReader> task)
	{
		if (task.IsCompletedSuccessfully)
		{
			DbDataReader reader = task.Result;
			while (reader?.Read() ?? false)
			{
				uint userId = (uint)(int)reader["user_id"];

				return userId;
			}
		}
		else if (task.IsFaulted)
		{
			UserManager.logger.LogError(EventIds.UserUpdateFailed, task.Exception, "Failed to get discord linkage from sql");
		}

		return  0;
	}

	private static (uint UserId, ulong DiscordId) ParseSqlGetDiscordLink2(Task<NpgsqlDataReader> task)
	{
		if (task.IsCompletedSuccessfully)
		{
			DbDataReader reader = task.Result;
			while (reader?.Read() ?? false)
			{
				uint userId = (uint)(int)reader["user_id"];
				ulong discordId = (ulong)(long)reader["discord_id"];

				return (userId, discordId);
			}
		}
		else if (task.IsFaulted)
		{
			UserManager.logger.LogError(EventIds.UserUpdateFailed, task.Exception, "Failed to get discord linkage from sql");
		}

		return default;
	}
}