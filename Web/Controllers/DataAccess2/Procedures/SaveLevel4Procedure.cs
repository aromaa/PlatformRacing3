using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Platform_Racing_3_Common.Database;
using Platform_Racing_3_Common.Level;
using Platform_Racing_3_Common.Customization;
using Platform_Racing_3_Web.Extensions;
using Platform_Racing_3_Web.Responses;
using Platform_Racing_3_Web.Responses.Procedures;
using Platform_Racing_3_Web.Utils;

namespace Platform_Racing_3_Web.Controllers.DataAccess2.Procedures
{
    public class SaveLevel4Procedure : IProcedure
    {
        private const uint TITLE_MIN_LENGTH = 1;
        private const uint TITLE_MAX_LENGTH = 50;

        private const uint DESCRIPTION_MAX_LENGTH = 250;

        private const uint CHANCE_MIN = 0;
        private const uint CHANCE_MAX = 100;

        private const uint SONG_ID_MIN = 0;
        private const uint SONG_ID_MAX = 26;

        private const uint BG_ID_MIN = 0;
        private const uint BG_ID_MAX = 11;

        private static readonly HashSet<string> SupportedItems = new HashSet<string>()
        {
            "li",
            "r",
            "fr",
            "l",
            "s",
            "po",
            "su",
            "na",
            "a",
            "sw",
            "b",
            "st",
            "t",
            "bo",
            "p",
            "sn",
            "j",
            "sp",
            "h",
            "lc",
        };

        public async Task<IDataAccessDataResponse> GetResponseAsync(HttpContext httpContext, XDocument xml)
        {
            uint userId = httpContext.IsAuthenicatedPr3User();
            if (userId > 0)
            {
                XElement data = xml.Element("Params");
                if (data != null)
                {
                    string ip = (string)data.Element("p_ip") ?? throw new DataAccessProcedureMissingData(); //Pretty much ignored

                    string title = (string)data.Element("p_title") ?? throw new DataAccessProcedureMissingData();
                    if (title.Length < SaveLevel4Procedure.TITLE_MIN_LENGTH || title.Length > SaveLevel4Procedure.TITLE_MAX_LENGTH)
                    {
                        return new DataAccessErrorResponse($"Level title must be between {SaveLevel4Procedure.TITLE_MIN_LENGTH} and {SaveLevel4Procedure.TITLE_MAX_LENGTH} chars long!");
                    }

                    string description = (string)data.Element("p_comment") ?? throw new DataAccessProcedureMissingData();
                    if (description.Length > SaveLevel4Procedure.DESCRIPTION_MAX_LENGTH)
                    {
                        return new DataAccessErrorResponse($"Level comment can't be longer than {SaveLevel4Procedure.DESCRIPTION_MAX_LENGTH} chars long!");
                    }

                    Bit publish = (Bit?)data.Element("p_publish") ?? throw new DataAccessProcedureMissingData();

                    string songId = (string)data.Element("p_song_id") ?? throw new DataAccessProcedureMissingData();
                    if (songId != "random" && !uint.TryParse(songId, out uint _))
                    {
                        return new DataAccessErrorResponse($"Song id must be 'random' or between ids of {SaveLevel4Procedure.SONG_ID_MIN} and {SaveLevel4Procedure.SONG_ID_MAX}");
                    }

                    string stringMode = (string)data.Element("p_mode") ?? throw new DataAccessProcedureMissingData();
                    if (!Enum.TryParse(stringMode, true, out LevelMode mode))
                    {
                        return new DataAccessErrorResponse("Invalid mode: " + stringMode);
                    }

                    uint seconds = (uint?)data.Element("p_seconds") ?? throw new DataAccessProcedureMissingData();
                    double gravity = (double?)data.Element("p_gravity") ?? throw new DataAccessProcedureMissingData();

                    float alien = (float?)data.Element("p_alien") ?? throw new DataAccessProcedureMissingData();
                    if (alien < SaveLevel4Procedure.CHANCE_MIN || alien > SaveLevel4Procedure.CHANCE_MAX)
                    {
                        return new DataAccessErrorResponse($"Alien chance must be between {SaveLevel4Procedure.CHANCE_MIN} and {SaveLevel4Procedure.CHANCE_MAX}");
                    }

                    float sfchm = (float?)data.Element("p_sfchm") ?? throw new DataAccessProcedureMissingData();
                    if (sfchm < SaveLevel4Procedure.CHANCE_MIN || sfchm > SaveLevel4Procedure.CHANCE_MAX)
                    {
                        return new DataAccessErrorResponse($"Alien chance must be between {SaveLevel4Procedure.CHANCE_MIN} and {SaveLevel4Procedure.CHANCE_MAX}");
                    }

                    float snow = (float?)data.Element("p_snow") ?? throw new DataAccessProcedureMissingData();
                    if (snow < SaveLevel4Procedure.CHANCE_MIN || snow > SaveLevel4Procedure.CHANCE_MAX)
                    {
                        return new DataAccessErrorResponse($"Alien chance must be between {SaveLevel4Procedure.CHANCE_MIN} and {SaveLevel4Procedure.CHANCE_MAX}");
                    }

                    float wind = (float?)data.Element("p_wind") ?? throw new DataAccessProcedureMissingData();
                    if (wind < SaveLevel4Procedure.CHANCE_MIN || wind > SaveLevel4Procedure.CHANCE_MAX)
                    {
                        return new DataAccessErrorResponse($"Alien chance must be between {SaveLevel4Procedure.CHANCE_MIN} and {SaveLevel4Procedure.CHANCE_MAX}");
                    }

                    string[] items = ((string)data.Element("p_items") ?? throw new DataAccessProcedureMissingData()).Split(',', StringSplitOptions.RemoveEmptyEntries).Distinct().Where((i) => SaveLevel4Procedure.SupportedItems.Contains(i)).ToArray(); //Remove all dublicated values and only include supported items

                    uint health;
                    if (mode == LevelMode.Deathmatch)
                    {
                        health = (uint?)data.Element("p_health") ?? throw new DataAccessProcedureMissingData();
                        if (health == 0)
                        {
                            return new DataAccessErrorResponse("Health must be positive");
                        }
                    }
                    else
                    {
                        health = 5;
                    }

                    int[] kingOfTheHat;
                    if (mode == LevelMode.KingOfTheHat)
                    {
                        string[] stringKingOfTheHat = ((string)data.Element("p_king_of_the_hat") ?? throw new DataAccessProcedureMissingData()).Split(':', StringSplitOptions.RemoveEmptyEntries);
                        if (stringKingOfTheHat.Length != 2)
                        {
                            return new DataAccessErrorResponse("Invalid king of the hat format");
                        }
                        else if (!uint.TryParse(stringKingOfTheHat[0], out uint kingOfTheHatId) || !Enum.IsDefined(typeof(Hat), kingOfTheHatId))
                        {
                            return new DataAccessErrorResponse("Invalid hat");
                        }
                        else if (!uint.TryParse(stringKingOfTheHat[1], out uint kingOfTheHatColor))
                        {
                            return new DataAccessErrorResponse("Invalid color");
                        }
                        else
                        {
                            kingOfTheHat = new int[] { (int)kingOfTheHatId, (int)kingOfTheHatColor };
                        }
                    }
                    else
                    {
                        kingOfTheHat = new int[] { (int)Hat.BaseballCap, Color.Green.ToArgb() };
                    }

                    string bgImage = (string)data.Element("p_bg_image") ?? throw new DataAccessProcedureMissingData();
                    if (!bgImage.StartsWith("BG"))
                    {
                        return new DataAccessErrorResponse("Background image data dont start with 'BG'");
                    }
                    else if (!uint.TryParse(bgImage.Substring(2), out uint bgImageId) || bgImageId < SaveLevel4Procedure.BG_ID_MIN || bgImageId > SaveLevel4Procedure.BG_ID_MAX)
                    {
                        return new DataAccessErrorResponse($"Background image id must be between of {SaveLevel4Procedure.BG_ID_MIN} and {SaveLevel4Procedure.BG_ID_MAX}");
                    }

                    string levelData = (string)data.Element("p_level_data") ?? throw new DataAccessProcedureMissingData();

                    uint levelId = await LevelManager.SaveLevelAsync(userId, title, description, publish, songId, mode, seconds, gravity, alien, sfchm, snow, wind, items, health, kingOfTheHat, bgImage, levelData);
                    if (levelId > 0)
                    {
                        return new DataAccessSaveLevel4Response(levelId);
                    }
                    else
                    {
                        return new DataAccessSaveLevel4Response();
                    }
                }
                else
                {
                    throw new DataAccessProcedureMissingData();
                }
            }
            else
            {
                return new DataAccessSaveLevel4Response();
            }
        }
    }
}
