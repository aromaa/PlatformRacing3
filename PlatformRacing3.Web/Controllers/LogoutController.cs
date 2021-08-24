using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace PlatformRacing3.Web.Controllers
{
    [ApiController]
    [Route("logout")]
    public class LogoutController : ControllerBase
    {
        public async Task LogoutAsync()
        {
            await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
