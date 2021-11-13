using Microsoft.AspNetCore.Mvc;
using PlatformRacing3.Common.User;
using PlatformRacing3.Web.Extensions;
using PlatformRacing3.Web.Responses;

namespace PlatformRacing3.Web.Controllers
{
	[ApiController]
    [Route("isloggedin")]
    [Produces("text/xml")]
    public class IsLoggedInController : ControllerBase
    {
        [HttpPost]
        public async Task<object> IsLoggedInAsync([FromQuery] string id, [FromForm] string gameId)
        {
            if (id == "IsLoggedIn" && gameId == "f1c25e3bd3523110394b5659c68d8092") //Sparkworks
            {
                uint userId = this.HttpContext.IsAuthenicatedPr3User();
                if (userId > 0)
                {
                    PlayerUserData playerUserData = await UserManager.TryGetUserDataByIdAsync(userId);
                    if (playerUserData != null)
                    {
                        return new IsLoggedInResponse(userId, playerUserData.Username);
                    }
                    else
                    {
                        return new DataAccessErrorResponse("We were unable to load your user data");
                    }
                }
                else
                {
                    return new IsLoggedInResponse();
                }
            }

            return this.BadRequest();
        }
    }
}
