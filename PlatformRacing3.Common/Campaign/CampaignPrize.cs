using System.Data.Common;
using System.Text.Json.Serialization;

namespace PlatformRacing3.Common.Campaign;

public class CampaignPrize
{
	[JsonPropertyName("id")]
	public uint Id { get; }
	[JsonIgnore]
	public CampaignPrizeType Type { get; }

	[JsonPropertyName("medals")]
	public uint MedalsRequired { get; }

	internal CampaignPrize(DbDataReader reader)
	{
		this.Id = (uint)(int)reader["id"];
		this.Type = (CampaignPrizeType)reader["type"];
		this.MedalsRequired = (uint)(int)reader["medals_required"];
	}

	[JsonPropertyName("category")]
	public string Category => this.Type.ToString().ToLowerInvariant();
}