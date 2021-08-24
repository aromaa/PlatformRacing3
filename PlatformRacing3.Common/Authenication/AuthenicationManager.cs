using System;
using System.Linq;
using System.Security.Cryptography;
using PlatformRacing3.Common.Redis;

namespace PlatformRacing3.Common.Authenication
{
    public class AuthenicationManager
    {
        private const string TOKEN_CHARS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const uint TOKEN_LENGTH = 64;

        public static string CreateUniqueLoginToken(uint userId)
        {
            if (userId == 0)
            {
                throw new ArgumentException(null, nameof(userId));
            }

            byte[] bytes = new byte[4]; //Four bytes should so it can be converted to int
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            Random random = new(BitConverter.ToInt32(bytes, 0)); //Secure random

            string token = new(Enumerable.Repeat(AuthenicationManager.TOKEN_CHARS, (int)AuthenicationManager.TOKEN_LENGTH).Select(s => s[random.Next(s.Length)]).ToArray());

            RedisConnection.GetDatabase().StringSet($"logintoken:{token}", userId, TimeSpan.FromSeconds(30));

            return token;
        }
    }
}
