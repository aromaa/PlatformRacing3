﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using PlatformRacing3.Common.Level;
using PlatformRacing3.Web.Responses;

namespace PlatformRacing3.Web.Controllers.Level;

[ApiController]
[Route("GetLevel")]
[Produces("text/xml")]
public class GetLevelController : ControllerBase
{
	[HttpGet]
	public async Task<object> GetLevelAsync([FromQuery] uint id, [FromQuery] uint version)
	{
		if (id == 0 || version == 0)
		{
			return this.BadRequest();
		}

		LevelData levelData = await LevelManager.GetLevelDataAsync(id, version);
		if (levelData != null)
		{
			this.HttpContext.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue()
			{
				MustRevalidate = true,

				Public = true,

				MaxAge = TimeSpan.FromHours(1),
				SharedMaxAge = TimeSpan.FromDays(1),
			};

			/*uint userId = httpContext.IsAuthenicatedPr3User();
			if (levelData.Publish || (userId > 0 && levelData.AuthorUserId == userId))
			{
			    return new DataAccessGetLevel2Response(levelData);
			}
			else
			{
			    return new DataAccessErrorResponse("You may not access unpublished levels!");
			}*/

			//TODO: Fix this, only allow accessing unpublished levels if going try matchlisting, can be easily done using redis tokens etc

			return new SingleRowResponse<LevelData>(levelData);
		}
		else
		{
			return this.NotFound();
		}
	}
}