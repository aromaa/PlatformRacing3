using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Platform_Racing_3_Common.Database;
using Platform_Racing_3_Common.Level;
using Platform_Racing_3_Common.Customization;
using Platform_Racing_3_Common.Utils;
using Platform_Racing_3_Server.Collections;
using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing;
using Platform_Racing_3_Server.Utils;
using Platform_Racing_3_UnsafeHelpers.Extensions;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Platform_Racing_3_Server.Game.Communication.Messages;
using Platform_Racing_3_Server.Game.Lobby;
using log4net;
using System.Reflection;

namespace Platform_Racing_3_Server.Game.Match
{
    internal class MultiplayerMatch
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        internal MatchListingType Type { get; }
        internal string Name { get; }

        internal LevelData LevelData { get; }

        private MultiplayerMatchStatus _Status;
        private ClientSessionCollection ReservedFor;
        private ClientSessionCollection Clients;

        private HashSet<uint> DrawingUsers;
        private ConcurrentDictionary<uint, MatchPlayer> Players;
        private Queue<MatchPlayer> PlayerOrderKeeper;

        private uint ReservedSlots;
        private uint UsersReady;

        private uint NextHatId;
        private ConcurrentDictionary<uint, MatchPlayerHat> DroppedHats;

        private MatchPrize Prize;
        private MatchTimer MatchTimer;

        private HashSet<Point> FinishBlocks;

        internal bool Broadcaster { get; set; }

        internal MultiplayerMatch(MatchListingType type, string name, LevelData levelData)
        {
            this.Type = type;

            this._Status = MultiplayerMatchStatus.PreparingForStart;
            this.ReservedFor = new ClientSessionCollection(this.OnReservedDisconnect);
            this.Clients = new ClientSessionCollection(this.OnDisconnect);

            this.DrawingUsers = new HashSet<uint>();
            this.Players = new ConcurrentDictionary<uint, MatchPlayer>();
            this.PlayerOrderKeeper = new Queue<MatchPlayer>();

            this.Name = name;
            this.LevelData = levelData;

            this.NextHatId = 0;
            this.DroppedHats = new ConcurrentDictionary<uint, MatchPlayerHat>();

            this.FinishBlocks = new HashSet<Point>();
        }

        public MultiplayerMatchStatus Status => this._Status;

        private void OnReservedDisconnect(ClientSession session)
        {
            InterlockedExtansions.Decrement(ref this.ReservedSlots);

            this.CheckGameState();
        }

        private void OnDisconnect(ClientSession session)
        {
            this.Leave0(session);
        }

        private uint GetNextHatId() => InterlockedExtansions.Increment(ref this.NextHatId);

        internal bool Reserve(ClientSession session)
        {
            if (this._Status != MultiplayerMatchStatus.PreparingForStart)
            {
                throw new InvalidOperationException();
            }

            if (this.ReservedFor.Add(session))
            {
                InterlockedExtansions.Increment(ref this.ReservedSlots);

                return true;
            }

            return false;
        }

        internal void Lock()
        {
            if (this.LevelData.Mode == LevelMode.KingOfTheHat)
            {
                if (InterlockedExtansions.CompareExchange(ref this._Status, MultiplayerMatchStatus.ServerDrawing, MultiplayerMatchStatus.PreparingForStart) == MultiplayerMatchStatus.PreparingForStart)
                {
                    Task.Run(this.DrawLevelAsync);

                    return;
                }
            }
            else
            {
                if (InterlockedExtansions.CompareExchange(ref this._Status, MultiplayerMatchStatus.WaitingForUsersToJoin, MultiplayerMatchStatus.PreparingForStart) == MultiplayerMatchStatus.PreparingForStart)
                {
                    this.CheckGameState();
                    return;
                }
            }

            throw new InvalidOperationException();
        }

        internal void Join(ClientSession session)
        {
            lock (((ICollection)this.PlayerOrderKeeper).SyncRoot) //Due to how things work we need to lock this whole block, TODO: looking for better solution
            {
                if (this.ReservedFor.Remove(session))
                {
                    if (this.Clients.Add(session) && this.DrawingUsers.Add(session.SocketId))
                    {
                        MatchPlayer matchPlayer = new MatchPlayer(this, session.UserData, session.SocketId, session.IPAddres);
                        if (this.Players.TryAdd(session.SocketId, matchPlayer))
                        {
                            session.MultiplayerMatchSession = new MultiplayerMatchSession(this, matchPlayer);

                            this.PlayerOrderKeeper.Enqueue(matchPlayer);

                            foreach (MatchPlayer player in this.PlayerOrderKeeper)
                            {
                                session.TrackUserInRoom(this.Name, player.SocketId, player.UserData.Id, player.UserData.Username, player.GetVars("speed", "accel", "jump", "hat", "head", "body", "feet", "hatColor", "headColor", "bodyColor", "feetColor"));

                                if (!this.DrawingUsers.Contains(player.SocketId))
                                {
                                    session.SendUserRoomData(this.Name, player.SocketId, new FinishDrawingOutgoingMessage(player.SocketId));
                                }
                            }

                            foreach (ClientSession other in this.Clients.Values.ToList())
                            {
                                other.TrackUserInRoom(this.Name, matchPlayer.SocketId, matchPlayer.UserData.Id, matchPlayer.UserData.Username, matchPlayer.GetVars("speed", "accel", "jump", "hat", "head", "body", "feet", "hatColor", "headColor", "bodyColor", "feetColor"));
                            }

                            InterlockedExtansions.Increment(ref this.UsersReady);

                            this.CheckGameState();
                        }
                        else
                        {
                            //Ehhh????
                        }
                    }
                }
                else
                {
                    //Spectate
                    this.Clients.Add(session);
                }
            }
        }

        internal void SendUpdateIfRequired(ClientSession session, MatchPlayer matchPlayer)
        {
            UpdateOutgoingMessage packet = matchPlayer.GetUpdatePacket();
            if (packet != null)
            {
                this.Clients.SendPacket(packet, session);
            }
        }

        internal void FinishDrawing(ClientSession session)
        {
            if (this.DrawingUsers.Remove(session.SocketId))
            {
                foreach(ClientSession other in this.Clients.Values)
                {
                    other.SendUserRoomData(this.Name, session.SocketId, new FinishDrawingOutgoingMessage(session.SocketId));
                }

                this.CheckGameState();
            }
        }

        internal void CheckGameState()
        {
            while (true)
            {
                if (this._Status == MultiplayerMatchStatus.WaitingForUsersToJoin)
                {
                    if (this.UsersReady == this.ReservedSlots)
                    {
                        InterlockedExtansions.CompareExchange(ref this._Status, MultiplayerMatchStatus.WaitingForUsersToDraw, MultiplayerMatchStatus.WaitingForUsersToJoin);
                    }
                    else
                    {
                        break;
                    }
                }
                else if (this._Status == MultiplayerMatchStatus.WaitingForUsersToDraw)
                {
                    if (this.DrawingUsers.Count == 0)
                    {
                        if (InterlockedExtansions.CompareExchange(ref this._Status, MultiplayerMatchStatus.Ongoing, MultiplayerMatchStatus.WaitingForUsersToDraw) == MultiplayerMatchStatus.WaitingForUsersToDraw)
                        {
                            this.Start();
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else if (this._Status == MultiplayerMatchStatus.Ongoing)
                {
                    bool timeRanOut = this.LevelData.Seconds > 0 && this.LevelData.Mode != LevelMode.KingOfTheHat ? this.MatchTimer.Elapsed.TotalSeconds > this.LevelData.Seconds : false;
                    if (timeRanOut)
                    {
                        if (this.LevelData.Mode == LevelMode.CoinFiend || this.LevelData.Mode == LevelMode.DamageDash)
                        {
                            foreach (ClientSession session in this.Clients.Values)
                            {
                                this.FinishMatch(session);
                            }
                        }
                    }
                    
                    if (timeRanOut || this.Players.Values.All((p) => p.Forfiet || p.FinishTime != null))
                    {
                        if (InterlockedExtansions.CompareExchange(ref this._Status, MultiplayerMatchStatus.Ended, MultiplayerMatchStatus.Ongoing) == MultiplayerMatchStatus.Ongoing)
                        {
                            this.Clients.SendPacket(new EndGameOutgoingMessage());
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else if (this._Status == MultiplayerMatchStatus.Ended)
                {
                    if (this.Clients.Count <= 0)
                    {
                        if (InterlockedExtansions.CompareExchange(ref this._Status, MultiplayerMatchStatus.Died, MultiplayerMatchStatus.Ended) == MultiplayerMatchStatus.Ended)
                        {
                            PlatformRacing3Server.MatchManager.Die(this);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        internal void Leave(ClientSession session)
        {
            if (this.Clients.Remove(session))
            {
                this.Leave0(session);
            }
        }

        internal void Start()
        {
            byte[] bytes = new byte[4];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create()) //Why not?
            {
                rng.GetBytes(bytes);
            }

            Random random = new Random(BitConverter.ToInt32(bytes, 0));

            HashSet<string> events = new HashSet<string>();
            if (this.LevelData.Snow > random.NextDouble() * 100)
            {
                events.Add("snow");
            }

            if (this.LevelData.Wind > random.NextDouble() * 100)
            {
                events.Add("wind");
            }

            //TODO: Move hat stuff to GetVars sometime
            if (this.LevelData.Mode != LevelMode.KingOfTheHat)
            {
                if (this.LevelData.Sfchm > random.NextDouble() * 100)
                {
                    events.Add("sfchm");

                    foreach (MatchPlayer player in this.Players.Values)
                    {
                        this.AddHatToPlayer(player, Hat.Cowboy, player.UserData.CurrentHatColor, false);
                    }
                }
                else
                {
                    foreach (MatchPlayer player in this.Players.Values)
                    {
                        if (player.UserData.CurrentHat != Hat.None)
                        {
                            this.AddHatToPlayer(player, player.UserData.CurrentHat, player.UserData.CurrentHatColor, false);
                        }
                        else if (this.LevelData.Mode == LevelMode.HatAttack)
                        {
                            this.AddHatToPlayer(player, Hat.CardboardBox, player.UserData.CurrentHatColor, false);
                        }
                    }
                }
            }
            else
            {
                Hat hat = (Hat)this.LevelData.KingOfTheHat[0];
                Color hatColor = Color.FromArgb((int)this.LevelData.KingOfTheHat[1]);

                foreach (Point point in this.FinishBlocks)
                {
                    this.DropHat(hat, hatColor, (point.X * 40) + 20, (point.Y * 40) + 20);
                }

                //Resend hats to avoid graphical bug on client side
                foreach (MatchPlayer player in this.Players.Values)
                {
                    this.Clients.SendPacket(new SetPlayerHatsOutgoingMessage(player.SocketId, player.Hats));
                }
            }

            if (this.LevelData.Alien > random.NextDouble() * 100)
            {
                events.Add("aliens");
            }

            if (this.LevelData.HasPrize && this.LevelData.Prizes.Count > 0)
            {
                (string prizeType, uint prizeId) = this.LevelData.Prizes[random.Next(this.LevelData.Prizes.Count)];

                this.Prize = new MatchPrize(prizeType, prizeId, rewardsExpBonus: false);
            }
            else if (this.Players.Count >= 2)
            {
                int diffIpsCount = this.Players.Values.Select((p) => p.IPAddress).Distinct().Count();
                if (diffIpsCount >= 2)
                {
                    double change = diffIpsCount * 5;
                    if (diffIpsCount >= 4)
                    {
                        if (random.Next(40 / diffIpsCount) == 0)
                        {
                            change *= 2;
                        }

                        if (this.Type == MatchListingType.Tournament)
                        {
                            change *= 2;
                        }
                        else if (this.Type == MatchListingType.LevelOfTheDay)
                        {
                            change *= 1.25;
                        }
                    }

                    if (change > random.NextDouble() * 100)
                    {
                        Part specialPartId = Part.None;
                        if (random.Next(2) == 0)
                        {
                            DateTimeOffset now = DateTimeOffset.UtcNow;
                            if (now >= new DateTimeOffset(now.Year, 10, 24, 0, 0, 0, TimeSpan.Zero) && now < new DateTimeOffset(now.Year, 11, 7, 0, 0, 0, TimeSpan.Zero)) //Halloween
                            {
                                specialPartId = Part.Ghost;
                            }
                            else if (now >= new DateTimeOffset(now.Year, 11, 17, 0, 0, 0, TimeSpan.Zero) && now < new DateTimeOffset(now.Year, 12, 1, 0, 0, 0, TimeSpan.Zero)) //Thanksgiving
                            {
                                specialPartId = Part.Turkey;
                            }
                            else if (now >= new DateTimeOffset(now.Year, 12, 1, 0, 0, 0, TimeSpan.Zero) && now < new DateTimeOffset(now.Year + 1, 1, 1, 0, 0, 0, TimeSpan.Zero)) //Christmas
                            {
                                specialPartId = Part.Reindeer;
                            }
                        }

                        if (specialPartId != Part.None)
                        {
                            uint headsCount = 0;
                            uint bodysCount = 0;
                            uint feetsCount = 0;
                            foreach(MatchPlayer player in this.Players.Values)
                            {
                                if (player.UserData.HasHead(specialPartId))
                                {
                                    headsCount++;
                                }
                                
                                if (player.UserData.HasBody(specialPartId))
                                {
                                    bodysCount++;
                                }

                                if (player.UserData.HasFeet(specialPartId))
                                {
                                    feetsCount++;
                                }
                            }

                            if (this.Players.Count != headsCount || this.Players.Count != bodysCount ||  this.Players.Count != feetsCount)
                            {
                                HashSet<string> possiblities = new HashSet<string>();
                                if (this.Players.Count != headsCount)
                                {
                                    possiblities.Add("head");
                                }

                                if (this.Players.Count != bodysCount)
                                {
                                    possiblities.Add("body");
                                }

                                if (this.Players.Count != feetsCount)
                                {
                                    possiblities.Add("feet");
                                }

                                this.Prize = new MatchPrize(possiblities.OrderBy((p) => random.NextDouble()).First(), (uint)specialPartId);
                            }
                        }

                        if (this.Prize == null)
                        {
                            if (random.Next(3333) == 0)
                            {
                                this.Prize = new MatchPrize("hat", (uint)new Hat[]
                                {
                                    Hat.None,
                                    Hat.BaseballCap,
                                    Hat.Cowboy,
                                    Hat.Crown
                                }.OrderBy((h) => random.NextDouble()).First());
                            }
                        }

                        if (this.Prize == null) //No special part was choosen, fallback to default
                        {
                            double prizeTypeChance = random.NextDouble() * 100;
                            if (prizeTypeChance >= 0 && prizeTypeChance <= 89) //Parts
                            {
                                Part part = new Part[]
                                {
                                    Part.Brain,
                                    Part.Cactus,
                                    Part.Cthulhu,
                                    Part.Dino,
                                    Part.Eye,
                                    Part.Hare,
                                    Part.Monster,
                                    Part.Mushroom,
                                    Part.Panda,
                                    Part.Platypus,
                                    Part.Robot,
                                    Part.Skeleton,
                                    Part.Spartan,
                                    Part.Tiki,
                                    Part.Tortoise,
                                    Part.Penguin,
                                    Part.Dragon,
                                    Part.Meteor,
                                    Part.Donkey,
                                    Part.Sword,
                                    Part.Snowman,
                                }.OrderBy((p) => random.NextDouble()).First();

                                if (prizeTypeChance > 40 && prizeTypeChance <= 67) //Body
                                {
                                    this.Prize = new MatchPrize("body", (uint)part);
                                }
                                else if (prizeTypeChance > 67 && prizeTypeChance <= 89) //Head
                                {
                                    this.Prize = new MatchPrize("head", (uint)part);
                                }
                                else //Feet
                                {
                                    this.Prize = new MatchPrize("feet", (uint)part);
                                }
                            }
                            else if (prizeTypeChance > 89 && prizeTypeChance <= 100) //Hat
                            {
                                Hat hat = new Hat[]
                                {
                                    Hat.Propeller,
                                    Hat.Santa,
                                    Hat.PedroTheSnail,
                                    Hat.Top,
                                    Hat.Party,
                                    Hat.Pirate,
                                    Hat.Nurse,
                                    Hat.Bouncy,
                                    Hat.Shark,
                                    Hat.Happy,
                                    Hat.Police,
                                    Hat.Ushanka,
                                    Hat.Toque,
                                    Hat.Fez,
                                    Hat.Witch,
                                    Hat.Halo,
                                }.OrderBy((h) => random.NextDouble()).First();

                                this.Prize = new MatchPrize("hat", (uint)hat);
                            }
                        }
                    }
                }
            }

            if (this.Prize != null)
            {
                this.Clients.SendPacket(new PrizeOutgoingMessage(this.Prize, "available"));
            }

            this.MatchTimer = MatchTimer.StartNew(TimeSpan.FromMilliseconds(2664));

            this.Clients.SendPackets(new EventsOutgoingMessage(events), new BeginMatchOutgoingMessage());

            LevelManager.AddPlaysAsync(this.LevelData.Id, (uint)this.Players.Count); //Lets do the most important thing now!
        }

        private async Task DrawLevelAsync()
        {
            try
            {
                this.FinishBlocks = new HashSet<Point>();

                if (this.LevelData.Data.StartsWith("v2 | "))
                {
                    JObject levelData = JsonConvert.DeserializeObject<JObject>(this.LevelData.Data.Substring(5));
                    if (levelData.TryGetValue("blockStr", out JToken jsonBlockStr))
                    {
                        string blockStr = (string)jsonBlockStr;
                        if (!string.IsNullOrWhiteSpace(blockStr))
                        {
                            uint blockId = 0;
                            int x = 0;
                            int y = 0;

                            HashSet<uint> blockIds = new HashSet<uint>();
                            Dictionary<Point, uint> blocks = new Dictionary<Point, uint>();
                            foreach (string block in blockStr.Split(','))
                            {
                                if (block[0] == 'b')
                                {
                                    blockId = uint.Parse(block.Substring(1));
                                    blockIds.Add(blockId);
                                }
                                else
                                {
                                    string[] coords = block.Split(':');

                                    x += int.Parse(coords[0]);
                                    y += int.Parse(coords[1]);

                                    blocks[new Point(x, y)] = blockId;
                                }
                            }

                            HashSet<uint> finishBlocks = new HashSet<uint>();
                            foreach (uint blockId_ in blockIds.ToList())
                            {
                                if (blockId_ < 700)
                                {
                                    if (blockId_ % 100 == 7)
                                    {
                                        finishBlocks.Add(blockId_);
                                    }

                                    blockIds.Remove(blockId_); //Known block, no need to load from db
                                }
                            }

                            //Not found in cached blocks
                            if (blockIds.Count > 0)
                            {
                                using (DatabaseConnection dbConnection = new DatabaseConnection())
                                {
                                    DbDataReader reader = await dbConnection.ReadDataAsync($"SELECT DISTINCT ON(id) id, settings FROM base.blocks WHERE id = ANY({blockIds.ToArray()}) ORDER BY id, version DESC LIMIT {blockIds.Count}");
                                    while (reader?.Read() ?? false)
                                    {
                                        uint id = (uint)(int)reader["id"];
                                        string settings = (string)reader["settings"];
                                        if (settings.StartsWith("v2 | "))
                                        {
                                            JObject blockData = JsonConvert.DeserializeObject<JObject>(settings.Substring(5));
                                            if (blockData.TryGetValue("left", out JToken sideSettings) && this.ReadBlockSideSettings(sideSettings))
                                            {
                                                finishBlocks.Add(id);
                                            }
                                            else if (blockData.TryGetValue("right", out sideSettings) && this.ReadBlockSideSettings(sideSettings))
                                            {
                                                finishBlocks.Add(id);
                                            }
                                            else if (blockData.TryGetValue("top", out sideSettings) && this.ReadBlockSideSettings(sideSettings))
                                            {
                                                finishBlocks.Add(id);
                                            }
                                            else if (blockData.TryGetValue("bottom", out sideSettings) && this.ReadBlockSideSettings(sideSettings))
                                            {
                                                finishBlocks.Add(id);
                                            }
                                            else if (blockData.TryGetValue("bump", out sideSettings) && this.ReadBlockSideSettings(sideSettings))
                                            {
                                                finishBlocks.Add(id);
                                            }
                                        }
                                    }
                                }
                            }

                            //We loaded everything
                            foreach (KeyValuePair<Point, uint> block in blocks)
                            {
                                if (finishBlocks.Contains(block.Value))
                                {
                                    this.FinishBlocks.Add(block.Key);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MultiplayerMatch.Logger.Error(ex);

                this.SendChatMessage("Broadcaster", 0, 0, Color.Red, $"Server side drawing failed");
            }
            finally
            {
                if (InterlockedExtansions.CompareExchange(ref this._Status, MultiplayerMatchStatus.WaitingForUsersToJoin, MultiplayerMatchStatus.ServerDrawing) == MultiplayerMatchStatus.ServerDrawing)
                {
                    this.CheckGameState();
                }
            }
        }

        private bool ReadBlockSideSettings(JToken sideSettings)
        {
            string type = (string)sideSettings["type"];
            if (type == "finish")
            {
                return true;
            }

            return false;
        }

        internal void AddHatToPlayer(MatchPlayer player, Hat hat, Color color, bool spawned = true)
        {
            player.AddHat(this.GetNextHatId(), hat, color, spawned);

            this.Clients.SendPacket(new SetPlayerHatsOutgoingMessage(player.SocketId, player.Hats));
        }

        internal void Forfiet(ClientSession session, bool leave = false)
        {
            if (leave)
            {
                session.MultiplayerMatchSession = null;
            }
            else
            {
                session.MultiplayerMatchSession.Forfiet();
            }

            if (this._Status < MultiplayerMatchStatus.Starting)
            {
                this.DrawingUsers.Remove(session.SocketId);
                this.Players.TryRemove(session.SocketId, out _);
            }
            else
            {
                if (this.Players.TryGetValue(session.SocketId, out MatchPlayer player))
                {
                    if (this.LevelData.Mode == LevelMode.HatAttack || this.LevelData.Mode == LevelMode.KingOfTheHat)
                    {
                        while (player.Hats.Count > 0)
                        {
                            this.ForceDropHat(player, 15);
                        }
                    }

                    //If player dosen't have finish time it should always be considered as forfiet
                    if (player.FinishTime == null)
                    {
                        if (!player.Forfiet && this.Broadcaster)
                        {
                            int place = this.Players.Count - this.Players.Values.Count((p) => p.Forfiet);

                            this.SendChatMessage("Broadcaster", 0, 0, Color.Red, $"User {player.UserData.Username} forfiet at place #{place}");
                        }

                        player.Forfiet = true;

                        this.Clients.SendPacket(new ForfietOutgoingMessage(session.SocketId));
                    }

                    if (leave)
                    {
                        player.Gone = true;
                    }

                    this.Clients.SendPacket(new PlayerFinishedOutgoingMessage(session.SocketId, (IReadOnlyCollection<MatchPlayer>)this.Players.Values));
                }
            }

            this.CheckGameState();
        }

        internal void ForceDropHat(MatchPlayer player, uint power = 7)
        {
            PointF rotatedPoint = Maths.RotatePoint(0, -60, -player.Rot);

            double x = player.X + rotatedPoint.X;
            double y = player.Y + rotatedPoint.Y;

            double throwAngle = Maths.DEG_RAD * new Random().NextDouble() * -180;
            float velX = (float)Math.Cos(throwAngle) * power;
            float velY = (float)Math.Sin(throwAngle) * power;

            velX = (float)Math.Round(velX * 100) / 100;
            velX = (float)Math.Round(velX * 100) / 100;

            this.LoseHat(player, x, y, velX, velY);
        }

        private void Leave0(ClientSession session)
        {
            this.Forfiet(session, true);

            foreach(ClientSession other in this.Clients.Values)
            {
                other.UntrackUserInRoom(this.Name, session.SocketId);
            }

            this.CheckGameState();
        }

        internal void FinishMatch(ClientSession session)
        {
            double now = this.MatchTimer.Elapsed.TotalSeconds;
            if (now < 0)
            {
                return;
            }

            if (this.LevelData.Seconds > 0 && this.LevelData.Mode != LevelMode.KingOfTheHat && now > (this.LevelData.Seconds + 5)) //Small threashold
            {
                return;
            }

            if (this.Players.TryGetValue(session.SocketId, out MatchPlayer player) && !player.Forfiet && player.FinishTime == null)
            {
                player.FinishTime = now;

                this.Clients.SendPacket(new PlayerFinishedOutgoingMessage(session.SocketId, (IReadOnlyCollection<MatchPlayer>)this.Players.Values));

                ulong expEarned = ExpUtils.GetExpEarnedForFinishing(now);

                List<object[]> expArray = new List<object[]>();
                if (this.LevelData.Mode == LevelMode.Race)
                {
                    expArray.Add(new object[] { "Level completed", expEarned });
                }
                else if (this.LevelData.Mode == LevelMode.Deathmatch)
                {
                    expArray.Add(new object[] { "Fighting spirit", expEarned });
                }
                else if (this.LevelData.Mode == LevelMode.HatAttack)
                {
                    expArray.Add(new object[] { "Hat owner", expEarned });
                }
                else if (this.LevelData.Mode == LevelMode.CoinFiend)
                {
                    expArray.Add(new object[] { "Coin meizer", expEarned });
                }
                else if (this.LevelData.Mode == LevelMode.DamageDash)
                {
                    expArray.Add(new object[] { "Damage dealer", expEarned });
                }
                else if (this.LevelData.Mode == LevelMode.KingOfTheHat)
                {
                    expArray.Add(new object[] { "Hat holder", expEarned });
                }

                int place = this.Players.Count;
                foreach (MatchPlayer other in this.Players.Values)
                {
                    if (other != player)
                    {
                        bool defeated = false;
                        switch (this.LevelData.Mode)
                        {
                            case LevelMode.Race:
                            case LevelMode.HatAttack:
                            case LevelMode.KingOfTheHat:
                                defeated = other.Forfiet || other.FinishTime == null;
                                break;
                            case LevelMode.Deathmatch:
                                defeated = other.Forfiet || other.FinishTime != null;
                                break;
                            case LevelMode.CoinFiend:
                                defeated = other.Forfiet || player.Coins > other.Coins;
                                break;
                            case LevelMode.DamageDash:
                                defeated = other.Forfiet || player.Dash > other.Dash;
                                break;
                        }

                        if (defeated)
                        {
                            place--;

                            ulong playerExp = (ulong)Math.Round(ExpUtils.GetExpForDefeatingPlayer(other.UserData.Rank) * ExpUtils.GetPlaytimeMultiplayer(other.FinishTime ?? now) * ExpUtils.GetKeyPressMultiplayer(other.KeyPresses));

                            if (other.IPAddress == player.IPAddress)
                            {
                                playerExp /= 2;
                            }

                            expEarned += playerExp;
                            expArray.Add(new object[] { "Defeated " + other.UserData.Username, playerExp });
                        }
                    }
                }

                ulong baseExp = expEarned;
                if (place == 1)
                {
                    MatchPrize prize = Interlocked.Exchange(ref this.Prize, null);
                    if (prize != null)
                    {
                        bool partExp = false;

                        if (prize.Category == "hat")
                        {
                            if (player.UserData.HasHat((Hat)prize.Id))
                            {
                                partExp = true;
                            }
                            else
                            {
                                player.UserData.GiveHat((Hat)prize.Id);
                            }
                        }
                        else if (prize.Category == "head")
                        {
                            if (player.UserData.HasHead((Part)prize.Id))
                            {
                                partExp = true;
                            }
                            else
                            {
                                player.UserData.GiveHead((Part)prize.Id);
                            }
                        }
                        else if (prize.Category == "body")
                        {
                            if (player.UserData.HasBody((Part)prize.Id))
                            {
                                partExp = true;
                            }
                            else
                            {
                                player.UserData.GiveBody((Part)prize.Id);
                            }
                        }
                        else if (prize.Category == "feet")
                        {
                            if (player.UserData.HasFeet((Part)prize.Id))
                            {
                                partExp = true;
                            }
                            else
                            {
                                player.UserData.GiveFeet((Part)prize.Id);
                            }
                        }

                        if (!prize.RewardsExpBonus)
                        {
                            partExp = false;
                        }

                        if (partExp)
                        {
                            expEarned += (ulong)Math.Round(baseExp * 0.5F);
                            expArray.Add(new object[] { "Prize bonus", "EXP X 1.5" });
                        }

                        session.SendPacket(new PrizeOutgoingMessage(prize, partExp ? "exp" : "won"));
                    }
                }

                float expHatBonus = 0;
                foreach (MatchPlayerHat hat in player.Hats)
                {
                    if (!hat.Spawned && hat.Hat == Hat.BaseballCap)
                    {
                        expHatBonus += expHatBonus == 0 ? 1F : 0.1F;
                    }
                }

                if (expHatBonus > 0)
                {
                    expEarned += (ulong)Math.Round(baseExp * expHatBonus);
                    expArray.Add(new object[] { "Exp hat", $"EXP X {expHatBonus + 1}" });
                }

                if (this.Type == MatchListingType.LevelOfTheDay)
                {
                    expEarned += (ulong)Math.Round(baseExp * 0.25F);
                    expArray.Add(new object[] { "LOTD bonus", "EXP X 1.25" });
                }

                ulong bonusExpDrained = 0;
                if (player.UserData.BonusExp > 0)
                {
                    bonusExpDrained = Math.Min(player.UserData.BonusExp, baseExp);
                    if (bonusExpDrained > 0)
                    {
                        expEarned += bonusExpDrained;
                        expArray.Add(new object[] { "Bonus exp", $"EXP X {(bonusExpDrained / baseExp) + 1}" });
                        expArray.Add(new object[] { "Bonus exp left", player.UserData.BonusExp - bonusExpDrained });

                        player.UserData.DrainBonusExp(bonusExpDrained);
                    }
                }

                session.SendPackets(new YouFinishedOutgoingMessage(session.UserData.Rank, session.UserData.Exp, ExpUtils.GetNextRankExpRequirement(session.UserData.Rank), expEarned, expArray));

                player.UserData.AddExp(expEarned);

                if (this.Broadcaster)
                {
                    this.SendChatMessage("Broadcaster", 0, 0, Color.Red, $"User {player.UserData.Username} finished at place #{place}");
                }

                this.CheckGameState();
            }
        }
        
        internal void TryGetItem(ClientSession session, int x, int y, string side, string item)
        {
            if (this.Players.TryGetValue(session.SocketId, out MatchPlayer player))
            {
                player.Item = item;

                this.SendUpdateIfRequired(session, player);
            }
        }

        internal void HandleData(ClientSession session, JsonSendToRoomIncomingMessage.RoomMessageData data, bool sendToSelf)
        {
            switch(data.Type)
            {
                case "chat":
                    {
                        if (!session.IsGuest)
                        {
                            this.SendChatMessage(session, (string)data.Data["message"], sendToSelf);
                        }
                    }
                    break;
                case "useItem":
                    {
                        this.SendUseItem(session, data.Data["p"].ToObject<double[]>(), sendToSelf);
                    }
                    break;
                case "shatterBlock":
                    {
                        this.SendShatterBlock(session, (int)data.Data["tileY"], (int)data.Data["tileX"], sendToSelf);
                    }
                    break;
                case "explodeBlock":
                    {
                        this.SendExplodeBlock(session, (int)data.Data["tileY"], (int)data.Data["tileX"], sendToSelf);
                    }
                    break;
            }
        }

        private void SendUseItem(ClientSession session, double[] pos, bool sendToSelf = false)
        {
            UseItemOutgoingMessage packet = new UseItemOutgoingMessage(this.Name, session.SocketId, pos);

            if (sendToSelf)
            {
                this.Clients.SendPacket(packet);
            }
            else
            {
                this.Clients.SendPacket(packet, session);
            }
        }

        private void SendChatMessage(ClientSession session, string message, bool sendToSelf = true)
        {
            if (message.StartsWith("/"))
            {
                string[] args = message.Substring(1).Split(' ');

                if (!PlatformRacing3Server.CommandManager.Execte(session, args[0], args.AsSpan().Slice(1, args.Length - 1)))
                {
                    session.SendPacket(new AlertOutgoingMessage("Unknown command"));
                }
            }
            else
            {
                ChatOutgoingMessage packet = new ChatOutgoingMessage(this.Name, message, session.SocketId, session.UserData.Id, session.UserData.Username, session.UserData.NameColor);

                if (sendToSelf)
                {
                    this.Clients.SendPacket(packet);
                }
                else
                {
                    this.Clients.SendPacket(packet, session);
                }
            }
        }

        private void SendChatMessage(string senderName, uint senderSocketId, uint senderUserId, Color senderNameColor, string message)
        {
            ChatOutgoingMessage packet = new ChatOutgoingMessage(this.Name, message, senderSocketId, senderUserId, senderName, senderNameColor);

            this.Clients.SendPacket(packet);
        }

        private void SendShatterBlock(ClientSession session, int tileY, int tileX, bool sendToSelf = false)
        {
            ShatterBlockOutgoingMessage packet = new ShatterBlockOutgoingMessage(this.Name, tileY, tileX);

            if (sendToSelf)
            {
                this.Clients.SendPacket(packet);
            }
            else
            {
                this.Clients.SendPacket(packet, session);
            }
        }

        private void SendExplodeBlock(ClientSession session, int tileY, int tileX, bool sendToSelf = false)
        {
            ExplodeBlockOutgoingMessage packet = new ExplodeBlockOutgoingMessage(this.Name, tileY, tileX);

            if (sendToSelf)
            {
                this.Clients.SendPacket(packet);
            }
            else
            {
                this.Clients.SendPacket(packet, session);
            }
        }

        internal void LoseHat(ClientSession session, double x, double y, float velX = 0, float velY = 0)
        {
            if (this.Players.TryGetValue(session.SocketId, out MatchPlayer player))
            {
                this.LoseHat(player, x, y, velX, velY);
            }
        }

        internal void LoseHat(MatchPlayer player, double x, double y, float velX = 0, float velY = 0)
        {
            MatchPlayerHat hat = player.RemoveFirstHat();
            if (hat != null)
            {
                this.DropHat(hat, x, y, velX, velY);
                this.Clients.SendPacket(new SetPlayerHatsOutgoingMessage(player.SocketId, player.Hats));
            }
        }

        internal void DropHat(Hat hat, Color color, double x, double y, float velX = 0, float velY = 0) => this.DropHat(new MatchPlayerHat(this.GetNextHatId(), hat, color, true), x, y, velX, velY);
        internal void DropHat(MatchPlayerHat hat, double x, double y, float velX = 0, float velY = 0)
        {
            if (this.DroppedHats.TryAdd(hat.Id, hat))
            {
                this.Clients.SendPacket(new AddHatOutgoingMessage(hat, x, y, velX, velY));
            }
            else
            {
                //Uhhh....
            }
        }

        internal void GetHat(ClientSession session, uint id)
        {
            if (this.Players.TryGetValue(session.SocketId, out MatchPlayer player))
            {
                if (!player.Forfiet && player.FinishTime == null)
                {
                    if (this.DroppedHats.TryRemove(id, out MatchPlayerHat hat))
                    {
                        player.AddHat(hat);

                        this.Clients.SendPacket(new RemoveHatOutgoingMessage(id), session);
                        this.Clients.SendPacket(new SetPlayerHatsOutgoingMessage(player.SocketId, player.Hats));
                    }
                }
            }
        }

        internal void UpdateCoins(ClientSession session, uint coins)
        {
            if (this.Players.TryGetValue(session.SocketId, out MatchPlayer player))
            {
                if (!player.Forfiet && player.FinishTime == null)
                {
                    player.Coins = coins;

                    this.Clients.SendPacket(new CoinsOutgoingMessage((IReadOnlyCollection<MatchPlayer>)this.Players.Values));
                }
            }
        }

        internal void UpdateDash(ClientSession session, uint dash)
        {
            if (this.Players.TryGetValue(session.SocketId, out MatchPlayer player))
            {
                if (!player.Forfiet && player.FinishTime == null)
                {
                    player.Dash = dash;

                    this.Clients.SendPacket(new CoinsOutgoingMessage((IReadOnlyCollection<MatchPlayer>)this.Players.Values));
                }
            }
        }

        internal void KothTime(ClientSession session, string time)
        {
            if (this.LevelData.Mode == LevelMode.KingOfTheHat)
            {
                if (this.Players.TryGetValue(session.SocketId, out MatchPlayer player))
                {
                    string[] time_ = time.Split(':');
                    if (time_.Length == 2)
                    {
                        if (!int.TryParse(time_[0], out int mins) || !int.TryParse(time_[1], out int secs))
                        {
                            return;
                        }
                        else if (mins < 0 || secs < 0)
                        {
                            mins = Math.Max(mins, 0);
                            secs = Math.Max(secs, 0);

                            time = $"{mins}:{secs.ToString("00")}";
                        }
                    }
                    else
                    {
                        return;
                    }

                    player.Koth = time;

                    //Coins packet does everything we want
                    this.Clients.SendPacket(new CoinsOutgoingMessage(new MatchPlayer[] { player }));
                }
            }
        }

        internal void SendPacket(IMessageOutgoing packet)
        {
            this.Clients.SendPacket(packet);
        }
    }
}
