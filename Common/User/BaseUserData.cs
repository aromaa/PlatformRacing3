using Platform_Racing_3_Common.Campaign;
using Platform_Racing_3_Common.Server;
using Platform_Racing_3_Common.Customization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace Platform_Racing_3_Common.User
{
    public abstract class BaseUserData : UserData
    {
        private static readonly Hat[] DefaultHats = new Hat[] { Hat.None };
        private static readonly Part[] DefaultHeads = new Part[] { Part.Alien, Part.Bigfoot, Part.Bird };
        private static readonly Part[] DefaultBodys = new Part[] { Part.Alien, Part.Bigfoot, Part.Bird };
        private static readonly Part[] DefaultFeets = new Part[] { Part.Alien, Part.Bigfoot, Part.Bird };

        public override string Status { get; protected set; }

        public override DateTimeOffset? LastLogin { get; protected set; }
        public override DateTimeOffset? LastOnline { get; protected set; }

        public override ulong TotalExp { get; protected set; }
        public override uint Rank { get; protected set; }
        public override ulong Exp { get; protected set; }
        public override ulong BonusExp { get; protected set; }

        protected HashSet<Hat> _Hats;
        protected HashSet<Part> _Heads;
        protected HashSet<Part> _Bodys;
        protected HashSet<Part> _Feets;

        public override Hat CurrentHat { get; protected set; }
        public override Color CurrentHatColor { get; protected set; }

        public override Part CurrentHead { get; protected set; }
        public override Color CurrentHeadColor { get; protected set; }

        public override Part CurrentBody { get; protected set; }
        public override Color CurrentBodyColor { get; protected set; }

        public override Part CurrentFeet { get; protected set; }
        public override Color CurrentFeetColor { get; protected set; }

        public override uint Speed { get; protected set; }
        public override uint Accel { get; protected set; }
        public override uint Jump { get; protected set; }

        protected Dictionary<uint, CampaignLevelRecord> _CampaignLevelRecords;

        protected HashSet<uint> _Friends;
        protected HashSet<uint> _Ignored;

        public BaseUserData()
        {
            this.Status = "Offline";

            this.LastLogin = DateTimeOffset.UtcNow;
            this.LastOnline = null;

            this.TotalExp = 0;
            this.Rank = 0;
            this.Exp = 0;
            this.BonusExp = 0;

            this._Hats = new HashSet<Hat>(GuestUserData.DefaultHats);
            this._Heads = new HashSet<Part>(GuestUserData.DefaultHeads);
            this._Bodys = new HashSet<Part>(GuestUserData.DefaultBodys);
            this._Feets = new HashSet<Part>(GuestUserData.DefaultFeets);

            Random random = new Random(); //By default randomize the look to make them look fancy :)

            this.CurrentHat = Hat.None;
            this.CurrentHatColor = Color.Black;

            this.CurrentHead = (Part)random.Next(1, 4);
            this.CurrentHeadColor = Color.FromArgb((int)Math.Round(random.NextDouble() * 16777215));

            this.CurrentBody = (Part)random.Next(1, 4);
            this.CurrentBodyColor = Color.FromArgb((int)Math.Round(random.NextDouble() * 16777215));

            this.CurrentFeet = (Part)random.Next(1, 4);
            this.CurrentFeetColor = Color.FromArgb((int)Math.Round(random.NextDouble() * 16777215));

            this.Speed = 50;
            this.Accel = 50;
            this.Jump = 50;

            this._CampaignLevelRecords = new Dictionary<uint, CampaignLevelRecord>();

            this._Friends = new HashSet<uint>();
            this._Ignored = new HashSet<uint>();
        }

        public override IReadOnlyCollection<Hat> Hats => this._Hats;
        public override IReadOnlyCollection<Part> Heads => this._Heads;
        public override IReadOnlyCollection<Part> Bodys => this._Bodys;
        public override IReadOnlyCollection<Part> Feets => this._Feets;

        public override IReadOnlyDictionary<uint, CampaignLevelRecord> CampaignLevelRecords => this._CampaignLevelRecords;

        public override IReadOnlyCollection<uint> Friends => this._Friends;
        public override IReadOnlyCollection<uint> Ignored => this._Ignored;

        //Override so for better performance
        public override void SetParts(Hat hat, Color hatColor, Part head, Color headColor, Part body, Color bodyColor, Part feet, Color feetColor)
        {
            if (this._Hats.Contains(hat))
            {
                this.CurrentHat = hat;
            }

            this.CurrentHatColor = hatColor;

            if (this._Heads.Contains(head))
            {
                this.CurrentHead = head;
            }

            this.CurrentHeadColor = headColor;

            if (this._Bodys.Contains(body))
            {
                this.CurrentBody = body;
            }

            this.CurrentBodyColor = bodyColor;

            if (this._Feets.Contains(feet))
            {
                this.CurrentFeet = feet;
            }

            this.CurrentFeetColor = feetColor;
        }

        public override bool HasHat(Hat hat) => this._Hats.Contains(hat);
        public override bool HasHead(Part head) => this._Heads.Contains(head);
        public override bool HasBody(Part body) => this._Bodys.Contains(body);
        public override bool HasFeet(Part feet) => this._Feets.Contains(feet);

        public override void GiveHat(Hat hat)
        {
            this._Hats.Add(hat);
        }

        public override void GiveHead(Part part)
        {
            this._Heads.Add(part);
        }

        public override void GiveBody(Part part)
        {
            this._Bodys.Add(part);
        }

        public override void GiveFeet(Part part)
        {
            this._Feets.Add(part);
        }

        public override void AddFriend(uint id) => this._Friends.Add(id);
        public override void RemoveFriend(uint id) => this._Friends.Remove(id);

        public override void AddIgnored(uint id) => this._Ignored.Add(id);
        public override void RemoveIgnored(uint id) => this._Ignored.Remove(id);
    }
}
