using Newtonsoft.Json;
using Platform_Racing_3_Common.Campaign;
using Platform_Racing_3_Common.Server;
using Platform_Racing_3_Common.Customization;
using Platform_Racing_3_Common.Utils;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Platform_Racing_3_Common.User
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public abstract class UserData : IXmlSerializable
    {
        public const uint DEFAULT_STATS_COUNT = 150;
        public const uint STATS_MIN = 0;
        public const uint STATS_MAX = 100;

        [JsonProperty("guest")]
        public abstract bool IsGuest { get; }

        [JsonProperty("userID")]
        public abstract uint Id { get; }
        [JsonProperty("userName")]
        public abstract string Username { get; protected set; }

        [JsonProperty("nameColor")]
        public abstract Color NameColor { get; protected set; }
        [JsonProperty("group")]
        public abstract string Group { get; protected set; }

        [JsonProperty("status")]
        public abstract string Status { get; protected set; }

        public abstract DateTimeOffset? LastLogin { get; protected set; }
        public abstract DateTimeOffset? LastOnline { get; protected set; }

        public abstract ulong TotalExp { get; protected set; }
        [JsonProperty("rank")]
        public abstract uint Rank { get; protected set; }
        [JsonProperty("exp")]
        public abstract ulong Exp { get; protected set; }
        public abstract ulong BonusExp { get; protected set; }

        [JsonProperty("hatArray")]
        public abstract IReadOnlyCollection<Hat> Hats { get; }
        [JsonProperty("headArray")]
        public abstract IReadOnlyCollection<Part> Heads { get; }
        [JsonProperty("bodyArray")]
        public abstract IReadOnlyCollection<Part> Bodys { get; }
        [JsonProperty("feetArray")]
        public abstract IReadOnlyCollection<Part> Feets { get; }

        [JsonProperty("hats")]
        public uint HatsCount => (uint)this.Hats.Count((h) => h != Hat.None);

        [JsonProperty("hat")]
        public abstract Hat CurrentHat { get; protected set; }
        [JsonProperty("hatColor")]
        public abstract Color CurrentHatColor { get; protected set; }

        [JsonProperty("head")]
        public abstract Part CurrentHead { get; protected set; }
        [JsonProperty("headColor")]
        public abstract Color CurrentHeadColor { get; protected set; }

        [JsonProperty("body")]
        public abstract Part CurrentBody { get; protected set; }
        [JsonProperty("bodyColor")]
        public abstract Color CurrentBodyColor { get; protected set; }

        [JsonProperty("feet")]
        public abstract Part CurrentFeet { get; protected set; }
        [JsonProperty("feetColor")]
        public abstract Color CurrentFeetColor { get; protected set; }

        [JsonProperty("speed")]
        public abstract uint Speed { get; protected set; }
        [JsonProperty("accel")]
        public abstract uint Accel { get; protected set; }
        [JsonProperty("jump")]
        public abstract uint Jump { get; protected set; }
        [JsonProperty("expBonus")]
        private uint __ExpBonus__UNUSED__ { get; } = 0;

        [JsonProperty("campaign")]
        public abstract IReadOnlyDictionary<uint, CampaignLevelRecord> CampaignLevelRecords { get; }

        public abstract IReadOnlyCollection<uint> Friends { get; }
        public abstract IReadOnlyCollection<uint> Ignored { get; }

        public abstract IReadOnlyCollection<string> Permissions { get; }

        private protected UserData()
        {
        }

        /// <summary>
        /// Jiggy
        /// </summary>
        [JsonProperty("prizes")]
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
            catch (StackOverflowException)
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
                    this.BonusExp += this.BonusExp;
                }
            }
            catch (StackOverflowException)
            {
                this.BonusExp = 0;
            }
        }

        public virtual void DrainBonusExp(ulong bonusExp)
        {
            try
            {
                checked
                {
                    this.BonusExp -= this.BonusExp;
                }
            }
            catch (StackOverflowException)
            {
                this.BonusExp = 0;
            }
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
}
