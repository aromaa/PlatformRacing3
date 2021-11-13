using System.Net.Mail;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using PlatformRacing3.Common.User;
using PlatformRacing3.Web.Extensions;

namespace PlatformRacing3.Web.Controllers;

[ApiController]
[Route("register")]
public class RegisterController : ControllerBase
{
	public const uint USERNAME_MIN_LENGTH = 1;
	public const uint USERNAME_MAX_LENGTH = 16;

	public const uint PASSWORD_MIN_LENGTH = 8;

	public const uint EMAIL_MAX_LENGTH = 255;

	private static readonly Regex UsernameRegex = new("^[a-zA-Z0-9-_]+$", RegexOptions.Compiled);

	[HttpPost]
	public async Task<string> RegisterAsync([FromForm] string username, [FromForm] string password, [FromForm(Name = "retype_password")] string retypePassword, [FromForm] string email)
	{
		//Username check
		if (string.IsNullOrWhiteSpace(username))
		{
			return "Please enter username";
		}
		else if (username.Length < RegisterController.USERNAME_MIN_LENGTH)
		{
			return $"Username must be at least {RegisterController.USERNAME_MIN_LENGTH} char(s)";
		}
		else if (username.Length > RegisterController.USERNAME_MAX_LENGTH)
		{
			return $"Username can't be longer than {RegisterController.USERNAME_MAX_LENGTH} char(s)";
		}
		else if (!RegisterController.UsernameRegex.IsMatch(username))
		{
			return "Username may only have characters are a-Z, 0-9, _ and -";
		}

		//Password check
		if (string.IsNullOrWhiteSpace(password))
		{
			return "Please enter password";
		}
		else if (password.Length < RegisterController.PASSWORD_MIN_LENGTH)
		{
			return $"Password must be at least {RegisterController.PASSWORD_MIN_LENGTH} chars long";
		}

		//Retype password check, this should be in client
		if (string.IsNullOrWhiteSpace(retypePassword))
		{
			return "Please retype your password";
		}
		else if (password != retypePassword)
		{
			return "Passwords don't match";
		}

		//Email check
		if (string.IsNullOrWhiteSpace(email))
		{
			return "Please enter email address, this can be used to recover your account";
		}
		else if (email.Length > RegisterController.EMAIL_MAX_LENGTH)
		{
			return $"Email address can't be longer than {RegisterController.EMAIL_MAX_LENGTH} char(s)";
		}
		else
		{
			try
			{
				new MailAddress(email); //TODO: Alternativly we could use regex
			}
			catch
			{
				return "Please enter valid email address";
			}
		}

		Task<PlayerUserData> userByName = UserManager.TryGetUserDataByNameAsync(username);
		Task<PlayerUserData> userByEmail = UserManager.TryGetUserDataByEmailAsync(email); //This speeds up things so lets do this?

		//Check for dublicated username
		if (await userByName != null)
		{
			return "Username has been taken already!";
		}

		//Check for dublicated email
		if (await userByEmail != null)
		{
			return "Email is linked to another user already!";
		}

		PlayerUserData userData = await UserManager.TryCreateNewUserAsync(username, password, email, this.HttpContext.Connection.RemoteIpAddress);
		if (userData == null)
		{
			return "Critical error while trying to create user! Please try again! If this error keeps happening, please report it to the staff team";
		}

		await this.HttpContext.LogOutPr3UserAsync(); //Log out to make sure authenicate passes successfully
		await this.HttpContext.AuthenicatePr3UserAsync(userData.Id); //Log in the user automatically after register

		return string.Empty; //Everything is fine, empty requests represents that there was no issues, bad design
	}
}