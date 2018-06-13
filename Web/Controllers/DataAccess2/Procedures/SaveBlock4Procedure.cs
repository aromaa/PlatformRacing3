using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Platform_Racing_3_Common.Block;
using Platform_Racing_3_Common.Database;
using Platform_Racing_3_Web.Extensions;
using Platform_Racing_3_Web.Responses;
using Platform_Racing_3_Web.Responses.Procedures;

namespace Platform_Racing_3_Web.Controllers.DataAccess2.Procedures
{
    public class SaveBlock4Procedure : IProcedure
    {
        private const uint TITLE_MIN_LENGTH = 1;
        private const uint TITLE_MAX_LENGTH = 50;
        
        private const uint CATEGORY_MAX_LENGTH = 50;

        private const uint DESCRIPTION_MAX_LENGTH = 250;

        private const uint SETTINGS_MAX_LENGTH = 1000;

        public async Task<IDataAccessDataResponse> GetResponse(HttpContext httpContext, XDocument xml)
        {
            uint userId = httpContext.IsAuthenicatedPr3User();
            if (userId > 0)
            {
                XElement data = xml.Element("Params");
                if (data != null)
                {
                    string ip = (string)data.Element("p_ip") ?? throw new DataAccessProcedureMissingData(); //Pretty much ignored
                    string title = (string)data.Element("p_title") ?? throw new DataAccessProcedureMissingData();
                    if (title.Length < SaveBlock4Procedure.TITLE_MIN_LENGTH || title.Length > SaveBlock4Procedure.TITLE_MAX_LENGTH)
                    {
                        return new DataAccessErrorResponse($"Block title must be between {SaveBlock4Procedure.TITLE_MIN_LENGTH} and {SaveBlock4Procedure.TITLE_MAX_LENGTH} chars long!");
                    }

                    string description = (string)data.Element("p_comment") ?? throw new DataAccessProcedureMissingData();
                    if (description.Length > SaveBlock4Procedure.DESCRIPTION_MAX_LENGTH)
                    {
                        return new DataAccessErrorResponse($"Block comment can't be longer than {SaveBlock4Procedure.DESCRIPTION_MAX_LENGTH} chars long!");
                    }

                    string imageData = (string)data.Element("p_image_data") ?? throw new DataAccessProcedureMissingData();
                    string settings = (string)data.Element("p_settings") ?? throw new DataAccessProcedureMissingData();
                    if (settings.Length > SaveBlock4Procedure.SETTINGS_MAX_LENGTH)
                    {
                        return new DataAccessErrorResponse($"Block settings is too big!");
                    }

                    string category = (string)data.Element("p_category") ?? throw new DataAccessProcedureMissingData();
                    if (description.Length > SaveBlock4Procedure.CATEGORY_MAX_LENGTH)
                    {
                        return new DataAccessErrorResponse($"Block category can't be longer than {SaveBlock4Procedure.CATEGORY_MAX_LENGTH} chars long!");
                    }

                    bool success = await BlockManager.SaveBlockAsync(userId, title, category, description, imageData, settings);
                    if (success)
                    {
                        return new DataAccessSaveBlock4Response(true);
                    }
                    else
                    {
                        return new DataAccessSaveBlock4Response();
                    }
                }
                else
                {
                    throw new DataAccessProcedureMissingData();
                }
            }
            else
            {
                return new DataAccessSaveBlock4Response();
            }
        }
    }
}
