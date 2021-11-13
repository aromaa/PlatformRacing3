using Microsoft.AspNetCore.Mvc;
using PlatformRacing3.Common.User;
using PlatformRacing3.Web.Extensions;

namespace PlatformRacing3.Web.Controllers;

[ApiController]
[Route("login")]
public class LoginController : ControllerBase
{
	[HttpPost]
	public async Task<string> LoginAsync([FromForm(Name = "username")] string identifier, [FromForm] string password)
	{
		if (string.IsNullOrWhiteSpace(identifier))
		{
			return "Please enter your username/email";
		}

		if (string.IsNullOrWhiteSpace(password))
		{
			return "Please enter your password";
		}

		uint userId = await UserManager.TryAuthenicateAsync(identifier, password);
		if (userId > 0)
		{
			//await this.HttpContext.LogOutPr3UserAsync(); //We can't log in as new user if they have authenicated already, so we should force log out the user?
			await this.HttpContext.AuthenicatePr3UserAsync(userId);

			return string.Empty; //Everything is ok, empty requests represents that there was no issues, bad design
		}
		else
		{
			return "Login details does not match, please check you entered the right username/email and password";
		}
	}
}