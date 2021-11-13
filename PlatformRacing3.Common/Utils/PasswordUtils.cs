using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace PlatformRacing3.Common.Utils;

internal static class PasswordUtils
{
	internal static string HashPassword(string password)
	{
		//Generate 128bit salt with secure random
		byte[] salt = new byte[128 / 8];
		using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
		{
			rng.GetBytes(salt);
		}
            
		//Generate password hash with 256bit subkey with 10k iteractions
		byte[] hash = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA1, 10000, 256 / 8);

		byte[] result = new byte[salt.Length + hash.Length + 1]; //+ 1 for version number
		result[0] = 0;

		Array.Copy(salt, 0, result, 1, salt.Length); //Copy salt to reuslt
		Array.Copy(hash, 0, result, salt.Length + 1, hash.Length); //Copy hash to result

		return Convert.ToBase64String(result);
	}

	internal static bool VerifyPassword(string password, string hash)
	{
		try
		{
			byte[] bytes = Convert.FromBase64String(hash);
			switch (bytes[0]) //Version number
			{
				case 0:
				{
					return PasswordUtils.VerifyPasswordVersion0(password, bytes);
				}
				default:
					return false;
			}
		}
		catch
		{
			return false;
		}
	}

	[Obsolete("This is legacy code")]
	internal static bool VerifyPasswordLegacy(string password, string hash)
	{
		//TEST PHP BCRYPT
		try
		{
			if (BCrypt.Net.BCrypt.Verify(password, hash))
			{
				return true;
			}
		}
		catch
		{

		}

		byte[] bytes = Encoding.UTF8.GetBytes(password);

		//MD5 (Don't worry, this was only for few months, was accidentally left after localhost testing)
		using (MD5 md5 = MD5.Create())
		{
			byte[] md5Hash = md5.ComputeHash(bytes);

			StringBuilder md5String = new();
			foreach(byte byte_ in md5Hash)
			{
				md5String.Append(byte_.ToString("x2"));
			}

			if (md5String.ToString() == hash)
			{
				return true;
			}
		}

		return false;
	}

	private static bool VerifyPasswordVersion0(string password, byte[] bytes)
	{
		//128bit salt
		byte[] salt = new byte[128 / 8];
		Array.Copy(bytes, 1, salt, 0, salt.Length);

		//Generate password hash with 256bit subkey with 10k iteractions
		byte[] hash = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA1, 10000, 256 / 8);

		byte[] dbHash = new byte[bytes.Length - salt.Length - 1];
		Array.Copy(bytes, salt.Length + 1, dbHash, 0, dbHash.Length);

		return hash.SequenceEqual(dbHash);
	}
}