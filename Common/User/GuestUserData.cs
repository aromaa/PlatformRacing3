using Newtonsoft.Json;
using Platform_Racing_3_Common.Campaign;
using Platform_Racing_3_Common.Server;
using Platform_Racing_3_Common.Customization;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace Platform_Racing_3_Common.User
{
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
}
