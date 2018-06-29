using Platform_Racing_3_Common.Campaign;
using Platform_Racing_3_Common.Database;
using Platform_Racing_3_Common.Level;
using Platform_Racing_3_Common.Redis;
using Platform_Racing_3_Common.Server;
using Platform_Racing_3_Common.Customization;
using Platform_Racing_3_Common.Utils;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Platform_Racing_3_Common.User
{
    public class PlayerUserData : BaseUserData
    {
        private static readonly Hat[] RestrictedHats = new Hat[] { Hat.Cowboy, Hat.Crown };
        private static readonly Part[] RestrictedParts = new Part[] { Part.Invisible, Part.MEME };

        public override bool IsGuest => false;

        public override uint Id { get; }
        public override string Username { get; }

        public uint PermissionRank { get; protected set; }
        public override Color NameColor { get; protected set; }
        public override string Group { get; protected set; }

        protected HashSet<string> _Permissions;

        private PlayerUserData()
        {
            this._Permissions = new HashSet<string>();
        }

        public PlayerUserData(DbDataReader reader) : this()
        {
            this.Id = (uint)(int)reader["id"];
            this.Username = (string)reader["username"];

            this.PermissionRank = (uint)(int)reader["permission_rank"];
            this.NameColor = Color.FromArgb((int)reader["name_color"]);
            this.Group = (string)reader["group_name"];
            
            object lastOnline = reader["last_online"]; //Might be null
            if (lastOnline is DateTime lastOnlineDate)
            {
                this.LastOnline = lastOnlineDate.ToUniversalTime();
            }

            this.SetTotalExp((ulong)(long)reader["total_exp"]); //Gonna calc the rank and exp
            this.BonusExp = (ulong)(long)reader["bonus_exp"];

            this._Hats.UnionWith(((int[])reader["hats"]).Select((h) => (Hat)h));
            this._Heads.UnionWith(((int[])reader["heads"]).Select((p) => (Part)p));
            this._Bodys.UnionWith(((int[])reader["bodys"]).Select((p) => (Part)p));
            this._Feets.UnionWith(((int[])reader["feets"]).Select((p) => (Part)p));

            Hat hat = (Hat)(int)reader["current_hat"];
            Color hatColor = Color.FromArgb((int)reader["current_hat_color"]);

            Part head = (Part)(int)reader["current_head"];
            Color headColor = Color.FromArgb((int)reader["current_head_color"]);

            Part body = (Part)(int)reader["current_body"];
            Color bodyColor = Color.FromArgb((int)reader["current_body_color"]);

            Part feet = (Part)(int)reader["current_feet"];
            Color feetColor = Color.FromArgb((int)reader["current_feet_color"]);

            this.SetParts(hat, hatColor, head, headColor, body, bodyColor, feet, feetColor);

            uint speed = (uint)(int)reader["speed"];
            uint accel = (uint)(int)reader["accel"];
            uint jump = (uint)(int)reader["jump"];

            this.SetStats(speed, accel, jump);

            object friends = reader["friends"]; //Might be null
            if (friends is int[] friends_)
            {
                this._Friends.UnionWith(friends_.Select((f) => (uint)f));
            }

            object ignored = reader["ignored"]; //Might be null
            if (ignored is int[] ignored_)
            {
                this._Ignored.UnionWith(ignored_.Select((i) => (uint)i));
            }
            
            object campaignRuns = reader["campaign_runs"]; //Might be null
            if (campaignRuns is int[,] campaignRuns_)
            {
                for(int i = 0; i < campaignRuns_.Length / campaignRuns_.Rank; i++)
                {
                    uint levelId = (uint)campaignRuns_[i, 0];
                    if (levelId > 0)
                    {
                        int finishTime = campaignRuns_[i, 1];
                        
                        this.CheckCampaignTime(levelId, finishTime);
                    }
                }
            }

            this.CheckCampaignPrizes();

            //TODO: Set up permissions based on permission rank

            this.RemoveRestrictedParts();
        }

        public PlayerUserData(RedisValue[] hashEntries) : this()
        {
            this.Id = (uint)hashEntries[0];
            this.Username = hashEntries[1];

            this.PermissionRank = (uint)hashEntries[2];
            this.NameColor = Color.FromArgb((int)hashEntries[3]);
            this.Group = hashEntries[4];

            this.LastLogin = DateTimeOffset.FromUnixTimeMilliseconds((long)hashEntries[5]);
            this.LastOnline = DateTimeOffset.FromUnixTimeMilliseconds((long)hashEntries[6]);

            this.SetTotalExp((ulong)hashEntries[7]); //Gonna calc the rank and exp
            this.BonusExp = (uint)hashEntries[8];

            this._Hats.UnionWith(hashEntries[9].ToString().Split(',').Select((h) => (Hat)uint.Parse(h)));
            this._Heads.UnionWith(hashEntries[10].ToString().Split(',').Select((p) => (Part)uint.Parse(p)));
            this._Bodys.UnionWith(hashEntries[11].ToString().Split(',').Select((p) => (Part)uint.Parse(p)));
            this._Feets.UnionWith(hashEntries[12].ToString().Split(',').Select((p) => (Part)uint.Parse(p)));

            Hat hat = (Hat)(uint)hashEntries[13];
            Color hatColor = Color.FromArgb((int)hashEntries[14]);

            Part head = (Part)(uint)hashEntries[15];
            Color headColor = Color.FromArgb((int)hashEntries[16]);

            Part body = (Part)(uint)hashEntries[17];
            Color bodyColor = Color.FromArgb((int)hashEntries[18]);

            Part feet = (Part)(uint)hashEntries[19];
            Color feetColor = Color.FromArgb((int)hashEntries[20]);

            this.SetParts(hat, hatColor, head, headColor, body, bodyColor, feet, feetColor);

            uint speed = (uint)hashEntries[21];
            uint accel = (uint)hashEntries[22];
            uint jump = (uint)hashEntries[23];

            this.SetStats(speed, accel, jump);

            this._Friends.UnionWith(hashEntries[25].ToString().Split(',').Select((f) => uint.Parse(f)));
            this._Ignored.UnionWith(hashEntries[26].ToString().Split(',').Select((i) => uint.Parse(i)));

            foreach (KeyValuePair<uint, int> record in hashEntries[24].ToString().Split(';').ToDictionary((l) => uint.Parse(l.Split(',')[0]), d => int.Parse(d.Split(',')[1])))
            {
                uint levelId = record.Key;
                int finishTime = record.Value;

                this.CheckCampaignTime(levelId, finishTime);
            }

            this.CheckCampaignPrizes();
        }

        private void CheckCampaignTime(uint levelId, int finishTime)
        {
            if (CampaignManager.DefaultCampaignTimes.TryGetValue(levelId, out Dictionary<CampaignMedal, uint> level))
            {
                if (this.CampaignLevelRecords.TryGetValue(levelId, out CampaignLevelRecord record_))
                {
                    if (finishTime < 0) //Died in deathmatch campaign
                    {
                        if (record_.Time > 0) //Current record is positive, so he finished, skip
                        {
                            return;
                        }
                        else if (record_.Time > finishTime) //Prefer longer time
                        {
                            return;
                        }
                    }
                    else if (finishTime > record_.Time) //Normal race, prefer shorter time
                    {
                        return;
                    }
                }

                CampaignMedal medal = CampaignMedal.None;
                if (finishTime > 0) //Only give medals for finished times
                {
                    if (level[CampaignMedal.Gold] > finishTime)
                    {
                        medal = CampaignMedal.Gold;
                    }
                    else if (level[CampaignMedal.Silver] > finishTime)
                    {
                        medal = CampaignMedal.Silver;
                    }
                    else if (level[CampaignMedal.Bronze] > finishTime)
                    {
                        medal = CampaignMedal.Bronze;
                    }
                }

                this._CampaignLevelRecords[levelId] = new CampaignLevelRecord(finishTime, medal);
            }
        }

        private void CheckCampaignPrizes()
        {
            uint goldMedalsCount = this.CampaignGoldMedals;
            foreach (CampaignPrize prize in CampaignManager.DefaultPrizes)
            {
                if (prize.Type == CampaignPrizeType.Hat)
                {
                    if (goldMedalsCount >= prize.MedalsRequired)
                    {
                        this._Hats.Add((Hat)prize.Id);
                    }
                    else
                    {
                        this._Hats.Remove((Hat)prize.Id);
                    }
                }
                else if (prize.Type == CampaignPrizeType.Head)
                {
                    if (goldMedalsCount >= prize.MedalsRequired)
                    {
                        this._Heads.Add((Part)prize.Id);
                    }
                    else
                    {
                        this._Heads.Remove((Part)prize.Id);
                    }
                }
                else if (prize.Type == CampaignPrizeType.Body)
                {
                    if (goldMedalsCount >= prize.MedalsRequired)
                    {
                        this._Bodys.Add((Part)prize.Id);
                    }
                    else
                    {
                        this._Bodys.Remove((Part)prize.Id);
                    }
                }
                else if (prize.Type == CampaignPrizeType.Feet)
                {
                    if (goldMedalsCount >= prize.MedalsRequired)
                    {
                        this._Feets.Add((Part)prize.Id);
                    }
                    else
                    {
                        this._Feets.Remove((Part)prize.Id);
                    }
                }
            }
        }

        private void RemoveRestrictedParts()
        {
            if (!this.HasPermissions("access_any_parts"))
            {
                this._Hats.RemoveWhere((h) => PlayerUserData.RestrictedHats.Contains(h));

                this._Heads.RemoveWhere((p) => PlayerUserData.RestrictedParts.Contains(p));
                this._Bodys.RemoveWhere((p) => PlayerUserData.RestrictedParts.Contains(p));
                this._Feets.RemoveWhere((p) => PlayerUserData.RestrictedParts.Contains(p));
            }
        }
        
        internal void Merge(PlayerUserData playerUserData)
        {
            this.PermissionRank = playerUserData.PermissionRank;
            this.NameColor = playerUserData.NameColor;
            this.Group = playerUserData.Group;

            this.LastLogin=playerUserData.LastLogin;
            this.LastOnline = playerUserData.LastOnline ;

            this.SetTotalExp(playerUserData.TotalExp); //Gonna calc the rank and exp
            this.BonusExp= playerUserData.BonusExp;

            this._Hats = playerUserData._Hats;
            this._Heads = playerUserData._Heads;
            this._Bodys = playerUserData._Bodys;
            this._Feets = playerUserData._Feets;

            this.SetParts(playerUserData.CurrentHat, playerUserData.CurrentHatColor, playerUserData.CurrentHead, playerUserData.CurrentHeadColor, playerUserData.CurrentBody, playerUserData.CurrentBodyColor, playerUserData.CurrentFeet, playerUserData.CurrentFeetColor);
            this.SetStats(playerUserData.Speed, playerUserData.Accel, playerUserData.Jump);

            this._CampaignLevelRecords = playerUserData._CampaignLevelRecords;

            this._Friends = playerUserData._Friends;
            this._Ignored = playerUserData._Ignored;
        }

        public override IReadOnlyCollection<string> Permissions => this._Permissions;

        public override void SetStats(uint speed, uint accel, uint jump)
        {
            base.SetStats(speed, accel, jump);

            UserManager.UpdateStatsAsync(this.Id, speed, accel, jump);
        }

        public override void SetParts(Hat hat, Color hatColor, Part head, Color headColor, Part body, Color bodyColor, Part feet, Color feetColor)
        {
            base.SetParts(hat, hatColor, head, headColor, body, bodyColor, feet, feetColor);

            UserManager.SetPartsAsync(this.Id, hat, hatColor, head, headColor, body, bodyColor, feet, feetColor);
        }

        public override void AddExp(ulong exp)
        {
            base.AddExp(exp);

            UserManager.AddExpAsync(this.Id, exp, this.TotalExp, this.Rank, this.Exp);
        }

        public override void DrainBonusExp(ulong bonusExp)
        {
            base.DrainBonusExp(bonusExp);

            UserManager.DrainBonusExp(this.Id, bonusExp, this.BonusExp);
        }

        public override bool HasHat(Hat hat) => this._Hats.Contains(hat);
        public override bool HasHead(Part head) => this._Heads.Contains(head);
        public override bool HasBody(Part body) => this._Bodys.Contains(body);
        public override bool HasFeet(Part feet) => this._Feets.Contains(feet);

        public override void GiveHat(Hat hat)
        {
            base.GiveHat(hat);

            UserManager.GiveHat(this.Id, hat);
        }

        public override void GiveHead(Part part)
        {
            base.GiveHead(part);

            UserManager.GiveHead(this.Id, part);
        }

        public override void GiveBody(Part part)
        {
            base.GiveBody(part);

            UserManager.GiveBody(this.Id, part);
        }

        public override void GiveFeet(Part part)
        {
            base.GiveFeet(part);

            UserManager.GiveFeet(this.Id, part);
        }

        public override void AddFriend(uint id)
        {
            base.AddFriend(id);

            UserManager.AddFriendAsync(this.Id, id);
        }

        public override void RemoveFriend(uint id)
        {
            base.RemoveFriend(id);

            UserManager.RemoveFriendAsync(this.Id, id);
        }

        public override void AddIgnored(uint id)
        {
            base.AddIgnored(id);

            UserManager.AddIgnoredAsync(this.Id, id);
        }

        public override void RemoveIgnored(uint id)
        {
            base.RemoveIgnored(id);

            UserManager.RemoveIgnoredAsync(this.Id, id);
        }

        public override bool HasPermissions(string permission) => this._Permissions.Contains(permission);

        //Called by UserManager after updating total exp to database and returning its result
        internal void SetTotalExp(ulong totalExp)
        {
            this.TotalExp = totalExp;

            //Lets recalc rank and exp, tho dont keep instantly update these to database if they are wrong, wait for AddExp to correct it
            (uint rank, ulong exp) = ExpUtils.GetRankAndExpFromTotalExp(totalExp);
            this.Rank = rank;
            this.Exp = exp;

            //Update to redis
            RedisConnection.GetDatabase().HashSetAsync($"users:{this.Id}", new HashEntry[]
            {
                new HashEntry("total_exp", totalExp),
                new HashEntry("rank", rank),
                new HashEntry("exp", exp),
            }, CommandFlags.FireAndForget);
        }

        //Called by UserManager after updating bonus exp to database and returning its result
        internal void SetTotalBonusExp(ulong bonusExp)
        {
            this.BonusExp = bonusExp;

            RedisConnection.GetDatabase().HashSetAsync($"users:{this.Id}", new HashEntry[]
            {
                new HashEntry("bonus_exp", bonusExp),
            }, CommandFlags.FireAndForget);
        }

        internal void SetHats(IEnumerable<Hat> hats)
        {
            this._Hats.UnionWith(hats);
            this._Hats.IntersectWith(hats);
        }

        internal void SetHeads(IEnumerable<Part> heads)
        {
            this._Heads.UnionWith(heads);
            this._Heads.IntersectWith(heads);
        }

        internal void SetBodys(IEnumerable<Part> bodys)
        {
            this._Bodys.UnionWith(bodys);
            this._Bodys.IntersectWith(bodys);
        }

        internal void SetFeets(IEnumerable<Part> feets)
        {
            this._Feets.UnionWith(feets);
            this._Feets.IntersectWith(feets);
        }

        public uint CampaignGoldMedals => (uint)this._CampaignLevelRecords.Values.Count((m) => m.Medal == CampaignMedal.Gold);

        public HashEntry[] ToRedis()
        {
            return new HashEntry[]
            {
                new HashEntry("id", this.Id),
                new HashEntry("username", this.Username),

                new HashEntry("permission_rank", this.PermissionRank),
                new HashEntry("name_color", this.NameColor.ToArgb()),
                new HashEntry("group_name", this.Group),

                new HashEntry("last_login", this.LastLogin.HasValue ? this.LastLogin.Value.ToUnixTimeMilliseconds() : RedisValue.EmptyString),
                new HashEntry("last_online", this.LastOnline.HasValue ? this.LastOnline.Value.ToUnixTimeMilliseconds() : RedisValue.EmptyString),

                new HashEntry("total_exp", this.TotalExp),
                new HashEntry("rank", this.Rank),
                new HashEntry("exp", this.Exp),
                new HashEntry("bonus_exp", this.BonusExp),

                new HashEntry("hats", string.Join(',', this._Hats.Select((h) => (uint)h))),
                new HashEntry("heads", string.Join(',', this._Heads.Select((p) => (uint)p))),
                new HashEntry("bodys", string.Join(',', this._Bodys.Select((p) => (uint)p))),
                new HashEntry("feets", string.Join(',', this._Feets.Select((p) => (uint)p))),

                new HashEntry("hat", (uint)this.CurrentHat),
                new HashEntry("hatcolor", this.CurrentHatColor.ToArgb()),

                new HashEntry("head", (uint)this.CurrentHead),
                new HashEntry("headcolor", this.CurrentHeadColor.ToArgb()),

                new HashEntry("body", (uint)this.CurrentBody),
                new HashEntry("bodycolor", this.CurrentBodyColor.ToArgb()),

                new HashEntry("feet", (uint)this.CurrentFeet),
                new HashEntry("feetcolor", this.CurrentFeetColor.ToArgb()),

                new HashEntry("speed", this.Speed),
                new HashEntry("accel", this.Accel),
                new HashEntry("jump", this.Jump),

                new HashEntry("campaigns", string.Join(';', this._CampaignLevelRecords.Select((c) => $"{c.Key},{c.Value.Time}"))),

                new HashEntry("friends", string.Join(',', this._Friends)),
                new HashEntry("ignored", string.Join(',', this._Ignored)),
            };
        }
    }
}
