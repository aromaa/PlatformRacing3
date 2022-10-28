using Microsoft.Extensions.FileSystemGlobbing.Internal;
using PlatformRacing3.Common.Database;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Reactive.Joins;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlatformRacing3.Server.Game.Level;

internal sealed class ServerLevelData
{
	internal Dictionary<Point, uint> BlockMap { get; }
	internal HashSet<uint> Blocks { get; }
	internal HashSet<uint> FinishBlocks { get; }

	internal HashSet<uint> PortableBlocks { get; }

	internal ServerLevelData(Dictionary<Point, uint> blockMap, HashSet<uint> blocks, HashSet<uint> finishBlocks, HashSet<uint> portableBlocks)
	{
		this.BlockMap = blockMap;
		this.Blocks = blocks;
		this.FinishBlocks = finishBlocks;
		this.PortableBlocks = portableBlocks;
	}

	internal static async Task<ServerLevelData> ParseLevelAsync(string data)
	{
		//TODO: Support not v2 types and optimize this
		
		if (data.StartsWith("v2 | "))
		{
			using JsonDocument levelData = JsonDocument.Parse(data.AsMemory(5));

			Dictionary<Point, uint> blockMap = new();
			HashSet<uint> blocksToFetch = new();
			HashSet<uint> blocks = new();

			HashSet<uint> finishBlocks = new();
			HashSet<uint> portableBlocks = new()
			{
				601
			};

			if (levelData.RootElement.TryGetProperty("blockStr", out JsonElement jsonBlockStr))
			{
				string blockStr = jsonBlockStr.GetString();
				if (!string.IsNullOrWhiteSpace(blockStr))
				{
					uint brushBlockId = 0;
					int brushX = 0;
					int brushY = 0;
					
					foreach (string block in blockStr.Split(','))
					{
						if (block[0] == 'b')
						{
							brushBlockId = uint.Parse(block.AsSpan(1));
							if (brushBlockId < 700)
							{
								if (brushBlockId % 100 == 7)
								{
									finishBlocks.Add(brushBlockId);
								}
							}
							else
							{
								blocks.Add(brushBlockId);
								blocksToFetch.Add(brushBlockId);
							}
						}
						else
						{
							string[] coords = block.Split(':');

							brushX += int.Parse(coords[0]);
							brushY += int.Parse(coords[1]);

							blockMap[new Point(brushX, brushY)] = brushBlockId;
						}
					}
				}
			}

			if (blocksToFetch.Count > 0)
			{
				using DatabaseConnection dbConnection = new();
				
				while (blocksToFetch.Count > 0)
				{
					await using DbDataReader reader = await dbConnection.ReadDataAsync($"SELECT DISTINCT ON(id) id, settings FROM base.blocks WHERE id = ANY({blocksToFetch.ToArray()}) ORDER BY id, version DESC LIMIT {blocksToFetch.Count}").ConfigureAwait(false);

					blocksToFetch.Clear();

					while (await reader.ReadAsync().ConfigureAwait(false))
					{
						uint id = (uint)(int)reader["id"];
						string settings = (string)reader["settings"];
						if (settings.StartsWith("v2 | "))
						{
							using JsonDocument blockData = JsonDocument.Parse(settings.AsMemory(5));

							string type = blockData.RootElement.GetProperty("type").GetString();
							if (type == "change")
							{
								foreach (JsonElement pattern in blockData.RootElement.GetProperty("changePattern").EnumerateArray())
								{
									uint blockId = pattern.GetUInt32();
									if (blocks.Add(blockId))
									{
										blocksToFetch.Add(blockId);
									}
								}
							}
							else if (type == "generator")
							{
								uint blockId = blockData.RootElement.GetProperty("generatorBlockID").GetUInt32();
								if (blocks.Add(blockId))
								{
									blocksToFetch.Add(blockId);
								}
							}

							if (blockData.RootElement.TryGetProperty("left", out JsonElement sideSettings))
							{
								ServerLevelData.ReadBlockSideSettings(id, blockData, sideSettings, blocksToFetch, blocks, finishBlocks, portableBlocks);
							}
							
							if (blockData.RootElement.TryGetProperty("right", out sideSettings))
							{
								ServerLevelData.ReadBlockSideSettings(id, blockData, sideSettings, blocksToFetch, blocks, finishBlocks, portableBlocks);
							}
							
							if (blockData.RootElement.TryGetProperty("top", out sideSettings))
							{
								ServerLevelData.ReadBlockSideSettings(id, blockData, sideSettings, blocksToFetch, blocks, finishBlocks, portableBlocks);
							}
							
							if (blockData.RootElement.TryGetProperty("bottom", out sideSettings))
							{
								ServerLevelData.ReadBlockSideSettings(id, blockData, sideSettings, blocksToFetch, blocks, finishBlocks, portableBlocks);
							}

							if (blockData.RootElement.TryGetProperty("bump", out sideSettings))
							{
								ServerLevelData.ReadBlockSideSettings(id, blockData, sideSettings, blocksToFetch, blocks, finishBlocks, portableBlocks);
							}
						}
					}
				}
			}

			return new ServerLevelData(blockMap, blocks, finishBlocks, portableBlocks);
		}

		return null;
	}

	private static void ReadBlockSideSettings(uint id, JsonDocument blockData, JsonElement sideSettings, HashSet<uint> blocksToFetch, HashSet<uint> blocks, HashSet<uint> finishBlocks, HashSet<uint> portableBlocks)
	{
		string type = sideSettings.GetProperty("type").GetString();
		if (type == "finish")
		{
			finishBlocks.Add(id);
		}
		else if (type == "customItem")
		{
			if (blockData.RootElement.TryGetProperty("itemType", out JsonElement itemType) && itemType.TryGetProperty("portableBlock", out JsonElement portableBlock))
			{
				uint blockId = portableBlock.GetUInt32();
				if (blocks.Add(blockId))
				{
					blocksToFetch.Add(blockId);
				}

				portableBlocks.Add(blockId);
			}
		}
	}
}
