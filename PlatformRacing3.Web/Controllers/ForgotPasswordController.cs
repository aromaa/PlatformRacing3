using System.Security.Cryptography;
using System.Text;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using MimeKit;
using PlatformRacing3.Common.User;

namespace PlatformRacing3.Web.Controllers;

[ApiController]
[Route("forgotpassword")]
public class ForgotPasswordController : ControllerBase
{
	[HttpPost]
	public async Task<string> PostAsync([FromForm] string email)
	{
		PlayerUserData player = await UserManager.TryGetUserDataByEmailAsync(email);
		if (player != null)
		{
			if (!await UserManager.CanSendPasswordToken(player.Id))
			{
				return "Please request new password reset token after half an hour!";
			}

			//Generate 512 bit random token
			byte[] token = new byte[512 / 8];

			using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
			{
				rng.GetNonZeroBytes(token.AsSpan(start: 0, length: 32));
			}

			using (SHA256 sha = SHA256.Create())
			{
				byte[] content = Encoding.UTF8.GetBytes(player.Id.ToString() + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + email);

				if (!sha.TryComputeHash(content, token.AsSpan(start: 32, length: 32), out int bytesWritten) || bytesWritten != 32)
				{
					//Should not happen, right?

					return "Something went wrong when trying to generate the token! Try again!";
				}
			}

			byte[] tokenHash;

			//Generate hash of the token for database
			using (SHA512 sha = SHA512.Create())
			{
				tokenHash = sha.ComputeHash(token);
			}

			bool result = await UserManager.TryCreateForgotPasswordToken(player.Id, Convert.ToBase64String(tokenHash), this.HttpContext.Connection.RemoteIpAddress);
			if (!result)
			{
				return "Something went wrong when trying to create the token! Try again!";
			}

			using (SmtpClient client = new())
			{
				await client.ConnectAsync(Program.Config.SmtpHost, Program.Config.SmtpPort).ConfigureAwait(false);
				await client.AuthenticateAsync(Program.Config.SmtpUser, Program.Config.SmtpPass).ConfigureAwait(false);

				using MimeMessage mail = new();
				mail.To.Add(new MailboxAddress(player.Username, email));
				mail.From.Add(new MailboxAddress(Program.Config.SmtpFrom, Program.Config.SmtpFrom));
				mail.Subject = $"Platform Racing 3 - {player.Username} - Password Reset";
				mail.Body = new TextPart("plain")
				{
					Text = $"You have requested password reset for your {player.Username} account! Use the following link to reset your password! https://pr3hub.com/resetpassword?email={email}&token={WebEncoders.Base64UrlEncode(token)}"
				};

				await client.SendAsync(mail).ConfigureAwait(false);
			}
			
			return "We have sent you email!";
		}
		else
		{
			return "User was not found!";
		}
	}
}