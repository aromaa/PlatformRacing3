using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Platform_Racing_3_Common.Level;
using Platform_Racing_3_Web.Extensions;

namespace Platform_Racing_3_Web.Controllers
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
