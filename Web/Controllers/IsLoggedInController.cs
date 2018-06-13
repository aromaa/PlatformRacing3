using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Platform_Racing_3_Common.User;
using Platform_Racing_3_Web.Extensions;
using Platform_Racing_3_Web.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Platform_Racing_3_Web.Controllers
{
    [Route("isloggedin")]
    [Produces("text/xml")]
    public class IsLoggedInController : Controller
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
