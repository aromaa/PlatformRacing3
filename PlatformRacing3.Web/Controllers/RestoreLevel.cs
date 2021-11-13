using Microsoft.AspNetCore.Mvc;
using PlatformRacing3.Common.Level;
using PlatformRacing3.Web.Extensions;

namespace PlatformRacing3.Web.Controllers
{
	[ApiController]
    [Route("restorelevel")]
    public class RestoreLevel : ControllerBase
    {
        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)] //Dynamic get
        public async Task<IActionResult> GetAll()
        {
            uint userId = this.HttpContext.IsAuthenicatedPr3User();
            if (userId == 0)
            {
                return this.Unauthorized();
            }

            return this.Ok(await LevelManager.GetDeletedLevels(userId));
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> RestoreLevelId(uint id, [FromForm] string rename)
        {
            uint userId = this.HttpContext.IsAuthenicatedPr3User();
            if (userId == 0)
            {
                return this.Unauthorized();
            }

            bool? success = await LevelManager.RestoreLevel(userId, id, rename);
            if (success == null)
            {
                return this.BadRequest();
            }

            if (success.Value)
            {
                return this.Ok();
            }
            else
            {
                return this.NotFound();
            }
        }
    }
}
