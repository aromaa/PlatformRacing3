using System.Drawing;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using PlatformRacing3.Common.Campaign;
using PlatformRacing3.Common.Customization;
using PlatformRacing3.Common.Server;
using PlatformRacing3.Common.Utils;

namespace PlatformRacing3.Common.User;

public abstract class UserData : IXmlSerializable
{
	public const uint DEFAULT_STATS_COUNT = 150;
	public const uint STATS_MIN = 0;
	public const uint STATS_MAX = 100;

	[JsonPropertyName("guest")]
	public abstract bool IsGuest { get; }

	[JsonPropertyName("userID")]
	public abstract uint Id { get; }
	[JsonPropertyName("userName")]
	public abstract string Username { get; protected set; }

	[JsonPropertyName("nameColor")]
	public abstract Color NameColor { get; protected set; }
	[JsonPropertyName("group")]
	public abstract string Group { get; protected set; }

	[JsonPropertyName("status")]
	public abstract string Status { get; protected set; }

	[JsonIgnore]
	public abstract DateTimeOffset? LastLogin { get; protected set; }
	[JsonIgnore]
	public abstract DateTimeOffset? LastOnline { get; protected set; }

	[JsonIgnore]
	public abstract ulong TotalExp { get; protected set; }
	[JsonPropertyName("rank")]
	public abstract uint Rank { get; protected set; }
	[JsonPropertyName("exp")]
	[JsonIgnore]
	public abstract ulong Exp { get; protected set; }
	[JsonIgnore]
	public abstract ulong BonusExp { get; protected set; }

	[JsonPropertyName("hatArray")]
	public abstract IReadOnlyCollection<Hat> Hats { get; }
	[JsonPropertyName("headArray")]
	public abstract IReadOnlyCollection<Part> Heads { get; }
	[JsonPropertyName("bodyArray")]
	public abstract IReadOnlyCollection<Part> Bodys { get; }
	[JsonPropertyName("feetArray")]
	public abstract IReadOnlyCollection<Part> Feets { get; }

	[JsonPropertyName("hats")]
	public uint HatsCount => (uint)this.Hats.Count((h) => h != Hat.None);

	[JsonPropertyName("hat")]
	public abstract Hat CurrentHat { get; protected set; }
	[JsonPropertyName("hatColor")]
	public abstract Color CurrentHatColor { get; protected set; }

	[JsonPropertyName("head")]
	public abstract Part CurrentHead { get; protected set; }
	[JsonPropertyName("headColor")]
	public abstract Color CurrentHeadColor { get; protected set; }

	[JsonPropertyName("body")]
	public abstract Part CurrentBody { get; protected set; }
	[JsonPropertyName("bodyColor")]
	public abstract Color CurrentBodyColor { get; protected set; }

	[JsonPropertyName("feet")]
	public abstract Part CurrentFeet { get; protected set; }
	[JsonPropertyName("feetColor")]
	public abstract Color CurrentFeetColor { get; protected set; }

	[JsonPropertyName("speed")]
	public abstract uint Speed { get; protected set; }
	[JsonPropertyName("accel")]
	public abstract uint Accel { get; protected set; }
	[JsonPropertyName("jump")]
	public abstract uint Jump { get; protected set; }
	[JsonPropertyName("expBonus")]
	private uint __ExpBonus__UNUSED__ { get; } = 0;

	[JsonIgnore]
	public abstract uint RadiatingLuck { get; protected set; }

	[JsonPropertyName("campaign")]
	public abstract IReadOnlyDictionary<uint, CampaignLevelRecord> CampaignLevelRecords { get; }

	[JsonIgnore]
	public abstract IReadOnlyCollection<uint> Friends { get; }
	[JsonIgnore]
	public abstract IReadOnlyCollection<uint> Ignored { get; }

	[JsonIgnore]
	public abstract IReadOnlyCollection<string> Permissions { get; }

	public bool Muted { get; set; }

	private protected UserData()
	{
	}

	/// <summary>
	/// Jiggy
	/// </summary>
	[JsonPropertyName("prizes")]
	public IReadOnlyDictionary<string, List<CampaignPrize>> Prizes => CampaignManager.DefaultPrizes;

	public virtual void SetServer(ServerDetails server)
	{
		if (server != null)
		{
			if (server.Name.Equals("The Factory", StringComparison.InvariantCultureIgnoreCase))
			{
				this.Status = $"On Lyoko";
			}
			else
			{
				this.Status = $"On {server.Name}";
			}
		}
		else
		{
			this.Status = "Offline";

			this.LastOnline = DateTimeOffset.Now;
		}
	}

	public virtual void SetStats(uint speed, uint accel, uint jump)
	{
		if (UserData.DEFAULT_STATS_COUNT + this.Rank >= speed + accel + jump)
		{
			if (speed >= UserData.STATS_MIN && speed <= UserData.STATS_MAX && accel >= UserData.STATS_MIN && accel <= UserData.STATS_MAX && jump >= UserData.STATS_MIN && jump <= UserData.STATS_MAX)
			{
				this.Speed = speed;
				this.Accel = accel;
				this.Jump = jump;
			}
		}
	}

	public virtual void SetParts(Hat hat, Color hatColor, Part head, Color headColor, Part body, Color bodyColor, Part feet, Color feetColor)
	{
		if (this.Hats.Contains(hat))
		{
			this.CurrentHat = hat;
		}

		this.CurrentHatColor = hatColor;

		if (this.Heads.Contains(head))
		{
			this.CurrentHead = head;
		}

		this.CurrentHeadColor = headColor;

		if (this.Bodys.Contains(body))
		{
			this.CurrentBody = body;
		}

		this.CurrentBodyColor = bodyColor;

		if (this.Feets.Contains(feet))
		{
			this.CurrentFeet = feet;
		}

		this.CurrentFeetColor = feetColor;
	}

	public virtual bool HasHat(Hat hat) => this.Hats.Contains(hat);
	public virtual bool HasHead(Part head) => this.Heads.Contains(head);
	public virtual bool HasBody(Part body) => this.Bodys.Contains(body);
	public virtual bool HasFeet(Part feet) => this.Feets.Contains(feet);

	public abstract void GiveHat(Hat hat, bool temporary = false);
	public abstract void GiveHead(Part part, bool temporary = false);
	public abstract void GiveBody(Part part, bool temporary = false);
	public abstract void GiveFeet(Part part, bool temporary = false);
	public virtual void GiveSet(Part part, bool temporary = false)
	{
		this.GiveHead(part, temporary);
		this.GiveBody(part, temporary);
		this.GiveFeet(part, temporary);
	}

	public abstract void RemoveHat(Hat hat, bool temporary = false);
	public abstract void RemoveHead(Part part, bool temporary = false);
	public abstract void RemoveBody(Part part, bool temporary = false);
	public abstract void RemoveFeet(Part part, bool temporary = false);
	public virtual void RemoveSet(Part part, bool temporary = false)
	{
		this.RemoveHead(part, temporary);
		this.RemoveBody(part, temporary);
		this.RemoveFeet(part, temporary);
	}

	public virtual void AddExp(ulong exp)
	{
		if (exp == 0 || this.TotalExp == ulong.MaxValue)
		{
			return;
		}

		try
		{
			checked
			{
				this.TotalExp += exp;
			}
		}
		catch (OverflowException)
		{
			this.TotalExp = ulong.MaxValue;
		}

		(uint rank, ulong expLeft) = ExpUtils.GetRankAndExpFromTotalExp(this.TotalExp);

		this.Rank = rank;
		this.Exp = expLeft;
	}

	public virtual void GiveBonusExp(ulong bonusExp)
	{
		try
		{
			checked
			{
				this.BonusExp += bonusExp;
			}
		}
		catch (OverflowException)
		{
			this.BonusExp = ulong.MaxValue;
		}
	}

	public virtual void DrainBonusExp(ulong bonusExp)
	{
		try
		{
			checked
			{
				this.BonusExp -= bonusExp;
			}
		}
		catch (OverflowException)
		{
			this.BonusExp = 0;
		}
	}

	public virtual bool DainLuckRadiation()
	{
		if (this.Rank < 5)
		{
			return false;
		}

		if (this.RadiatingLuck > 0)
		{
			this.RadiatingLuck--;

			return true;
		}

		return false;
	}

	public abstract void AddFriend(uint id);
	public abstract void RemoveFriend(uint id);

	public abstract void AddIgnored(uint id);
	public abstract void RemoveIgnored(uint id);

	public virtual void CheckCampaignPrizes() => this.CheckCampaignPrizes(null, 0);
	public abstract void CheckCampaignPrizes(string season, uint medalsCount);

	public virtual bool HasPermissions(string permission) => this.Permissions.Contains(permission);

	public IReadOnlyDictionary<string, object> GetVars(params string[] vars) => JsonUtils.GetVars(this, vars);
	public IReadOnlyDictionary<string, object> GetVars(HashSet<string> vars) => JsonUtils.GetVars(this, vars);

	public XmlSchema GetSchema() => null;

	public void ReadXml(XmlReader reader) => throw new NotSupportedException();
	public void WriteXml(XmlWriter writer)
	{
		writer.WriteElementString("userID", this.Id.ToString());
		writer.WriteElementString("userName", this.Username);

		writer.WriteElementString("nameColor", this.NameColor.ToArgb().ToString());

		writer.WriteElementString("rank", this.Rank.ToString());

		writer.WriteElementString("hat_array", string.Join(',', this.Hats));

		writer.WriteElementString("status", this.Status);
	}
}