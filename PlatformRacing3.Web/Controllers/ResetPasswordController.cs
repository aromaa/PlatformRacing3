using System.Security.Cryptography;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using MimeKit;
using PlatformRacing3.Common.User;

namespace PlatformRacing3.Web.Controllers;

[ApiController]
[Route("resetpassword")]
public class ResetPasswordController : ControllerBase
{
	[HttpPost]
	public async Task<string> PostAsync([FromForm] string email, [FromForm] string token, [FromForm] string password)
	{
		//Password check
		if (string.IsNullOrWhiteSpace(password))
		{
			return "Please enter password";
		}
		else if (password.Length < RegisterController.PASSWORD_MIN_LENGTH)
		{
			return $"Password must be at least {RegisterController.PASSWORD_MIN_LENGTH} chars long";
		}

		PlayerUserData player = await UserManager.TryGetUserDataByEmailAsync(email);
		if (player == null)
		{
			return "Invalid token";
		}

		string hashedToken;
		using (SHA512 sha = SHA512.Create())
		{
			hashedToken = Convert.ToBase64String(sha.ComputeHash(WebEncoders.Base64UrlDecode(token)));
		}

		uint userId = await UserManager.TryGetForgotPasswordToken(hashedToken);
		if (userId == 0 || userId != player.Id)
		{
			return "Invalid token";
		}

		if (!await UserManager.TryConsumePasswordToken(hashedToken, password, this.HttpContext.Connection.RemoteIpAddress))
		{
			return "Error, try again";
		}

		using (SmtpClient client = new())
		{
			await client.ConnectAsync(Program.Config.SmtpHost, Program.Config.SmtpPort).ConfigureAwait(false);
			await client.AuthenticateAsync(Program.Config.SmtpUser, Program.Config.SmtpPass).ConfigureAwait(false);

			using MimeMessage mail = new();
			mail.To.Add(new MailboxAddress(player.Username, email));
			mail.From.Add(new MailboxAddress(Program.Config.SmtpFrom, Program.Config.SmtpFrom));
			mail.Subject = $"Platform Racing 3 - {player.Username} - Security Alert";
			mail.Body = new TextPart("plain")
			{
				Text = $"Your account {player.Username} password was reset! If you did not do this change your password immediately!"
			};

			await client.SendAsync(mail).ConfigureAwait(false);
		}

		return "Password has been reset! Start the grind :sunglasses:";
	}
}