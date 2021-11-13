using System.Security.Cryptography;
using PlatformRacing3.Common.Redis;
using StackExchange.Redis;

namespace PlatformRacing3.Common.Authenication;

public class AuthenicationManager
{
	private const int TOKEN_LENGTH = 64;

	public static async Task<string> CreateUniqueLoginTokenAsync(uint userId)
	{
		if (userId == 0)
		{
			throw new ArgumentException(null, nameof(userId));
		}
            
		static string GenerateToken()
		{
			return string.Create(AuthenicationManager.TOKEN_LENGTH, (object)null, (span, unused) =>
			{
				Span<byte> bytes = stackalloc byte[AuthenicationManager.TOKEN_LENGTH / 4 * 3];

				RandomNumberGenerator.Fill(bytes);

				Convert.TryToBase64Chars(bytes, span, out _);
			});
		}

		while (true)
		{
			string token = GenerateToken();

			bool success = await RedisConnection.GetDatabase().StringSetAsync(new RedisKey("logintoken:").Append(token), userId, TimeSpan.FromSeconds(30), When.NotExists);
			if (success)
			{
				return token;
			}
		}
	}
}