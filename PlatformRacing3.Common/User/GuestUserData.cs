using System.Drawing;
using PlatformRacing3.Common.Campaign;

namespace PlatformRacing3.Common.User;

public class GuestUserData : BaseUserData
{
	private static readonly string GuestGroup = "Guest";
	private static readonly Color GuestColor = Color.FromArgb(29, 84, 151);

	private static readonly IReadOnlyList<string> EmptyPermissionList = new List<string>().AsReadOnly();
        
	public override bool IsGuest => true;

	public override uint Id => 0u;
	public override string Username { get; protected set; }

	public override Color NameColor
	{
		get => GuestUserData.GuestColor;
		protected set
		{
			//Do nothing, lets keep guests name colors the default to avoid confusion
		}
	}

	public override string Group
	{
		get => GuestUserData.GuestGroup;
		protected set
		{
			//Do nothing, lets keep guests group name the default to avoid confusion
		}
	}

	public override string Status { get; protected set; }

	public override DateTimeOffset? LastOnline
	{
		get => null;
		protected set
		{
			//Do nothing, lets keep guest last online the default to avoid confusion
		}
	}

	public override uint RadiatingLuck
	{
		get => 0u;
		protected set
		{
			//Do nothing
		}
	}

	public GuestUserData(uint guestId) : base()
	{
		this.Username = "Guest_" + guestId;

		this.Status = "Online";
	}

	public override IReadOnlyCollection<string> Permissions => GuestUserData.EmptyPermissionList; //Guests dont have rights! Boo those alien noobs!
	public override bool HasPermissions(string permission) => false;

	public override void CheckCampaignPrizes()
	{

	}

	public override void CheckCampaignPrizes(string season, uint medalsCount)
	{
		CampaignManager.AwardCampaignPrizes(this, season ?? "reborn", medalsCount);
	}
}