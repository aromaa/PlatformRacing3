using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Mobile_Import.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Platform_Racing_3_Common.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace Mobile_Import
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Write("Author: ");
            uint authorId = uint.Parse(Console.ReadLine());

            DatabaseConfig config = JsonConvert.DeserializeObject<DatabaseConfig>(File.ReadAllText("settings.json"));

            Console.WriteLine("Connecting to database... ");
            DatabaseConnection.Init(config);

            List<FileInfo> blocks = new();
            List<FileInfo> levels = new();
            FileInfo levelList = null;

            foreach (string path in Directory.GetFiles("Data"))
            {
                FileInfo file = new(path);
                if (file.Name.StartsWith("Block"))
                {
                    blocks.Add(file);
                }
                else if (file.Name.StartsWith("LevelList"))
                {
                    if (levelList != null)
                    {
                        throw new Exception("Multiple LevelList");
                    }

                    levelList = file;
                }
                else if (file.Name.StartsWith("Level"))
                {
                    levels.Add(file);
                }
                else
                {
                    Console.WriteLine($"Unknown file: {path}");
                }
            }

            IDictionary<uint, string> levelsByBlockId = new Dictionary<uint, string>();

            foreach (FileInfo file in levels)
            {
                JObject level = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(file.ToString()));

                string title = (string)level["title"];

                string levelData = (string)level["levelData"];
                if (levelData.StartsWith("v2 | "))
                {
                    levelData = levelData[5..];
                }
                else
                {
                    try
                    {
                        using (MemoryStream compressedMemoryStream = new(Convert.FromBase64String(levelData)))
                        {
                            using (InflaterInputStream inflater = new(compressedMemoryStream))
                            {
                                using (MemoryStream uncompressedMemoryStream = new())
                                {
                                    inflater.CopyTo(uncompressedMemoryStream);

                                    levelData = Encoding.UTF8.GetString(uncompressedMemoryStream.ToArray());
                                }
                            }
                        }
                    }
                    catch
                    {

                    }
                }

                JObject levelDataJson = JsonConvert.DeserializeObject<JObject>(levelData);
                string[] blockStr = ((string)levelDataJson["blockStr"]).Split(',');
                foreach (string block in blockStr)
                {
                    if (block.StartsWith('b'))
                    {
                        uint blockId = uint.Parse(block[1..]);

                        if (levelsByBlockId.TryGetValue(blockId, out string blockLevel))
                        {
                            //if (blockLevel != title)
                            //{
                            //    throw new Exception("Multiple levels");
                            //}
                        }
                        else
                        {
                            levelsByBlockId.Add(blockId, title);
                        }
                    }
                }
            }

            IDictionary<uint, uint> newIds = new Dictionary<uint, uint>();
            ISet<string> dbNames = new HashSet<string>();

            List<FileInfo> changeBlocks = new();

            using (DatabaseConnection dbConnection = new())
            {
                Console.WriteLine("Connected to database");

                dbConnection.ExecuteNonQuery($"DELETE FROM base.levels l USING base.levels_titles t WHERE l.id = t.id AND t.author_user_id = {authorId}");
                dbConnection.ExecuteNonQuery($"DELETE FROM base.levels_titles WHERE author_user_id = {authorId}");

                dbConnection.ExecuteNonQuery($"DELETE FROM base.blocks b USING base.blocks_titles t WHERE b.id = t.id AND t.author_user_id = {authorId}");
                dbConnection.ExecuteNonQuery($"DELETE FROM base.blocks_titles WHERE author_user_id = {authorId}");

                foreach (FileInfo file in blocks)
                {
                    XDocument xml;
                    using (FileStream stream = file.OpenRead())
                    {
                        xml = XDocument.Load(stream);
                    }

                    XElement row = xml.Element("Row");
                    if (row != null)
                    {
                        uint blockId = (uint)row.Element("block_id");
                        uint version = (uint)row.Element("version");

                        string title = (string)row.Element("title");
                        string comment = (string)row.Element("comment");

                        string lastUpdate = (string)row.Element("last_updated");

                        string settings = (string)row.Element("settings");
                        string imageData = (string)row.Element("image_data");

                        DateTime lastUpdateDateTime = DateTime.Parse(lastUpdate);

                        if (!levelsByBlockId.TryGetValue(blockId, out string levelTitle))
                        {
                            levelTitle = "unused";
                        }

                        string dbName = title[..Math.Min(title.Length, 50)];
                        if (!dbNames.Add($"{dbName}-{levelTitle}"))
                        {
                            for (int i = 2; i <= 5; i++)
                            {
                                if (dbNames.Add($"{dbName}-{levelTitle}-{i}"))
                                {
                                    levelTitle += $"-{i}";
                                    break;
                                }
                            }
                        }

                        string settingsParsed;
                        if (settings.StartsWith("v2 | "))
                        {
                            settingsParsed = settings[5..];
                        }
                        else
                        {
                            try
                            {
                                using (MemoryStream compressedMemoryStream = new(Convert.FromBase64String(settings)))
                                {
                                    using (InflaterInputStream inflater = new(compressedMemoryStream))
                                    {
                                        using (MemoryStream uncompressedMemoryStream = new())
                                        {
                                            inflater.CopyTo(uncompressedMemoryStream);

                                            settingsParsed = Encoding.UTF8.GetString(uncompressedMemoryStream.ToArray());
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                settingsParsed = settings;
                            }
                        }

                        JObject settingsJson = JsonConvert.DeserializeObject<JObject>(settingsParsed);
                        if ((string)settingsJson["type"] == "change")
                        {
                            changeBlocks.Add(file);

                            continue;
                        }

                        uint newId = (uint)(int)dbConnection.ExecuteScalar($"INSERT INTO base.blocks_titles(title, category, author_user_id) VALUES({dbName}, {levelTitle}, {authorId}) RETURNING id");

                        dbConnection.ExecuteNonQuery($"INSERT INTO base.blocks(id, version, description, image_data, settings, last_updated) VALUES({newId}, {version}, {comment}, {imageData}, {settings}, {lastUpdateDateTime})");

                        newIds.Add(blockId, newId);
                    }
                    else
                    {
                        Console.WriteLine($"File {file} is missing Row");
                    }
                }

                foreach (FileInfo file in changeBlocks)
                {
                    XDocument xml;
                    using (FileStream stream = file.OpenRead())
                    {
                        xml = XDocument.Load(stream);
                    }

                    XElement row = xml.Element("Row");
                    if (row != null)
                    {
                        uint blockId = (uint)row.Element("block_id");
                        uint version = (uint)row.Element("version");

                        string title = (string)row.Element("title");
                        string comment = (string)row.Element("comment");

                        string lastUpdate = (string)row.Element("last_updated");

                        string settings = (string)row.Element("settings");
                        string imageData = (string)row.Element("image_data");

                        DateTime lastUpdateDateTime = DateTime.Parse(lastUpdate);

                        if (!levelsByBlockId.TryGetValue(blockId, out string levelTitle))
                        {
                            levelTitle = "unused";
                        }

                        string dbName = title[..Math.Min(title.Length, 50)];
                        if (!dbNames.Add($"{dbName}-{levelTitle}"))
                        {
                            for (int i = 2; i <= 5; i++)
                            {
                                if (dbNames.Add($"{dbName}-{levelTitle}-{i}"))
                                {
                                    levelTitle += $"-{i}";
                                    break;
                                }
                            }
                        }

                        string settingsParsed;
                        if (settings.StartsWith("v2 | "))
                        {
                            settingsParsed = settings[5..];
                        }
                        else
                        {
                            try
                            {
                                using (MemoryStream compressedMemoryStream = new(Convert.FromBase64String(settings)))
                                {
                                    using (InflaterInputStream inflater = new(compressedMemoryStream))
                                    {
                                        using (MemoryStream uncompressedMemoryStream = new())
                                        {
                                            inflater.CopyTo(uncompressedMemoryStream);

                                            settingsParsed = Encoding.UTF8.GetString(uncompressedMemoryStream.ToArray());
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                settingsParsed = settings;
                            }
                        }

                        JObject settingsJson = JsonConvert.DeserializeObject<JObject>(settingsParsed);
                        if ((string)settingsJson["type"] != "change")
                        {
                            throw new Exception();
                        }

                        uint[] changePattern = settingsJson["changePattern"].ToObject<uint[]>();
                        for (int i = 0; i < changePattern.Length; i++)
                        {
                            if (newIds.TryGetValue(changePattern[i], out uint newChangeId))
                            {
                                changePattern[i] = newChangeId;
                            }
                            else if (blockId > 600)
                            {
                                changePattern[i] = 9999; //No block
                            }
                        }

                        settingsJson["changePattern"] = JToken.FromObject(changePattern);

                        settings = "v2 | " + JsonConvert.SerializeObject(settingsJson);

                        uint newId = (uint)(int)dbConnection.ExecuteScalar($"INSERT INTO base.blocks_titles(title, category, author_user_id) VALUES({dbName}, {levelTitle}, {authorId}) RETURNING id");

                        dbConnection.ExecuteNonQuery($"INSERT INTO base.blocks(id, version, description, image_data, settings, last_updated) VALUES({newId}, {version}, {comment}, {imageData}, {settings}, {lastUpdateDateTime})");

                        newIds.Add(blockId, newId);
                    }
                    else
                    {
                        Console.WriteLine($"File {file} is missing Row");
                    }
                }

                IDictionary<uint, (uint Version, uint Bronze, uint Silver, uint Gold, uint MedalsRequired, uint Time)> levelsData = new Dictionary<uint, (uint, uint, uint, uint, uint, uint)>();

                JObject[] levelListJson = JsonConvert.DeserializeObject<JObject[]>(File.ReadAllText(levelList.ToString()));
                foreach (JObject levelListLevel in levelListJson)
                {
                    uint levelId = (uint)levelListLevel["levelID"];
                    uint version = (uint)levelListLevel["version"];

                    uint bronze = (uint)levelListLevel["bronze"];
                    uint silver = (uint)levelListLevel["silver"];
                    uint gold = (uint)levelListLevel["gold"];
                    uint medalsRequired = (uint)levelListLevel["medalsRequired"];

                    uint time = (uint)levelListLevel["time"];

                    levelsData.Add(levelId, (version, bronze, silver, gold, medalsRequired, time));
                }

                foreach (FileInfo file in levels)
                {
                    JObject level = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(file.ToString()));

                    uint levelId = (uint)level["levelID"];

                    string title = (string)level["title"];
                    string comment = (string)level["comment"];

                    string[] items = ((string)level["items"]).Split(',');

                    uint seconds = (uint)level["seconds"];
                    string songId = (string)level["songID"];

                    double gravity = (double)level["gravity"];

                    string bgImage = (string)level["bgImage"];

                    string levelData = (string)level["levelData"];
                    if (levelData.StartsWith("v2 | "))
                    {
                        levelData = levelData[5..];
                    }
                    else
                    {
                        try
                        {
                            using (MemoryStream compressedMemoryStream = new(Convert.FromBase64String(levelData)))
                            {
                                using (InflaterInputStream inflater = new(compressedMemoryStream))
                                {
                                    using (MemoryStream uncompressedMemoryStream = new())
                                    {
                                        inflater.CopyTo(uncompressedMemoryStream);

                                        levelData = Encoding.UTF8.GetString(uncompressedMemoryStream.ToArray());
                                    }
                                }
                            }
                        }
                        catch
                        {

                        }
                    }

                    JObject levelDataJson = JsonConvert.DeserializeObject<JObject>(levelData);
                    string[] blockStr = ((string)levelDataJson["blockStr"]).Split(',');
                    for (int i = 0; i < blockStr.Length; i++)
                    {
                        string block = blockStr[i];
                        if (block.StartsWith('b'))
                        {
                            uint blockId = uint.Parse(block[1..]);
                            if (newIds.TryGetValue(blockId, out uint newId))
                            {
                                blockStr[i] = "b" + newId;
                            }
                            else if (blockId > 600)
                            {
                                blockStr[i] = "b9999"; //No block
                            }
                        }
                    }

                    levelDataJson["blockStr"] = JToken.FromObject(blockStr);

                    levelData = "v2 | " + JsonConvert.SerializeObject(levelDataJson);

                    if (!levelsData.TryGetValue(levelId, out (uint Version, uint Bronze, uint Silver, uint Gold, uint MedalsRequired, uint Time) levelExtraData))
                    {
                        throw new Exception();
                    }

                    uint newLevelId = (uint)(int)dbConnection.ExecuteScalar($"INSERT INTO base.levels_titles(title, author_user_id) VALUES({title}, {authorId}) RETURNING id");

                    dbConnection.ExecuteNonQuery($"INSERT INTO base.levels(id, version, description, publish, song_id, mode, seconds, gravity, alien, sfchm, snow, wind, items, health, king_of_the_hat, bg_image, level_data, last_updated) VALUES({newLevelId}, {levelExtraData.Version}, {comment}, true, {songId}, 'race', {seconds}, {gravity}, 0, 0, 0, 0, {items}, 5, '{{2,-16744448}}'::int[], {bgImage}, {levelData}, {DateTimeOffset.FromUnixTimeSeconds(levelExtraData.Time)})");
                }
            }

            Console.WriteLine("Done!");
            Console.ReadKey();
        }
    }
}
