using PlatformRacing3.Common.Level;
using PlatformRacing3.Web.Controllers.DataAccess2.Procedures.Exceptions;
using PlatformRacing3.Web.Extensions;
using PlatformRacing3.Web.Responses;
using PlatformRacing3.Web.Responses.Procedures;
using System.Xml.Linq;
using PlatformRacing3.Common.User;

namespace PlatformRacing3.Web.Controllers.DataAccess2.Procedures;

public sealed class PurchaseItemProcedure : IProcedure
{
	public async Task<IDataAccessDataResponse> GetResponseAsync(HttpContext httpContext, XDocument xml)
	{
		uint userId = httpContext.IsAuthenicatedPr3User();
		if (userId == 0)
		{
			return new DataAccessErrorResponse("You are not logged in!");
		}

		XElement data = xml.Element("Params");
		if (data is null)
		{
			throw new DataAccessProcedureMissingData();
		}

		uint levelId = (uint?)data.Element("p_level_id") ?? throw new DataAccessProcedureMissingData();
		string itemId = (string)data.Element("p_item_id") ?? throw new DataAccessProcedureMissingData();
		uint price = (uint?)data.Element("p_price") ?? throw new DataAccessProcedureMissingData();

		if (itemId.Length > 64)
		{
			return new DataAccessErrorResponse("Purchase item id too long");
		}

		if (price is <= 0 or > 100 * 100)
		{
			return new DataAccessErrorResponse("Invalid item price!");
		}

		bool result;
		if (levelId == 0)
		{
			result = await UserManager.UseCoins(userId, price);
		}
		else
		{
			if (price == 1) //No tax
			{
				result = await UserManager.PurchaseLevelItem(userId, levelId, itemId, price, price);
			}
			else
			{
				result = await UserManager.PurchaseLevelItem(userId, levelId, itemId, price, price - (uint) Math.Ceiling(price * 0.02));
			}
		}

		if (!result)
		{
			return new DataAccessErrorResponse("Purchase failed!");
		}

		return new DataAccessPurchaseItemResponse();
	}
}
