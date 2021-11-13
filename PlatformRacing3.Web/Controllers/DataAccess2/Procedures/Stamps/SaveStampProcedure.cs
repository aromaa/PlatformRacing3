using System.Xml.Linq;
using PlatformRacing3.Common.Stamp;
using PlatformRacing3.Web.Controllers.DataAccess2.Procedures.Exceptions;
using PlatformRacing3.Web.Extensions;
using PlatformRacing3.Web.Responses;
using PlatformRacing3.Web.Responses.Procedures.Stamps;

namespace PlatformRacing3.Web.Controllers.DataAccess2.Procedures.Stamps;

public class SaveStampProcedure : IProcedure
{
	private const uint TITLE_MIN_LENGTH = 1;
	private const uint TITLE_MAX_LENGTH = 50;

	private const uint CATEGORY_MAX_LENGTH = 50;

	private const uint DESCRIPTION_MAX_LENGTH = 250;

	public async Task<IDataAccessDataResponse> GetResponseAsync(HttpContext httpContext, XDocument xml)
	{
		uint userId = httpContext.IsAuthenicatedPr3User();
		if (userId > 0)
		{
			XElement data = xml.Element("Params");
			if (data != null)
			{
				string title = (string)data.Element("p_title") ?? throw new DataAccessProcedureMissingData();
				if (title.Length < SaveStampProcedure.TITLE_MIN_LENGTH || title.Length > SaveStampProcedure.TITLE_MAX_LENGTH)
				{
					return new DataAccessErrorResponse($"Stamp title must be between {SaveStampProcedure.TITLE_MIN_LENGTH} and {SaveStampProcedure.TITLE_MAX_LENGTH} chars long!");
				}

				string description = (string)data.Element("p_comment") ?? throw new DataAccessProcedureMissingData();
				if (description.Length > SaveStampProcedure.DESCRIPTION_MAX_LENGTH)
				{
					return new DataAccessErrorResponse($"Stamp comment can't be longer than {SaveStampProcedure.DESCRIPTION_MAX_LENGTH} chars long!");
				}

				string category = (string)data.Element("p_category") ?? throw new DataAccessProcedureMissingData();
				if (category.Length > SaveStampProcedure.CATEGORY_MAX_LENGTH)
				{
					return new DataAccessErrorResponse($"Stamp category can't be longer than {SaveStampProcedure.CATEGORY_MAX_LENGTH} chars long!");
				}

				string art = (string)data.Element("p_art") ?? throw new DataAccessProcedureMissingData();

				bool success = await StampManager.SaveStampAsync(userId, title, category, description, art);
				if (success)
				{
					return new DataAccessSaveStampResponse(true);
				}
				else
				{
					return new DataAccessSaveStampResponse();
				}
			}
			else
			{
				throw new DataAccessProcedureMissingData();
			}
		}
		else
		{
			return new DataAccessSaveStampResponse();
		}
	}
}