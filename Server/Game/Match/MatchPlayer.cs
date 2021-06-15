using Newtonsoft.Json;
using Platform_Racing_3_Common.User;
using Platform_Racing_3_Common.Customization;
using Platform_Racing_3_Common.Utils;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Enums;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Packets.Match;

namespace Platform_Racing_3_Server.Game.Match
{
    internal class MatchPlayer
    {
        internal MultiplayerMatch Match { get; } //IMatch?
        internal UserData UserData { get; }

        internal uint SocketId { get; }
        internal IPAddress IPAddress { get; }

        [JsonProperty("speed")]
        internal int _Speed;
        [JsonProperty("accel")]
        internal int _Accel;
        [JsonProperty("jump")]
        internal int _Jump;

        private Queue<MatchPlayerHat> _Hats { get; }

        //TODO: Mess
        private double _X;
        private double _Y;
        private float _VelX;
        private float _VelY;
        private byte _ScaleX;

        private bool _Space;
        private bool _Left;
        private bool _Right;
        private bool _Down;
        private bool _Up;

        private float _Rot;
        private string _Item;
        private uint _Life;
        private bool _Hurt;
        private uint _Coins;
        private uint _Dash;
        private string _Team;

        internal double? FinishTime { get; set; }
        internal int? FinishPlace { get; set; }
        internal bool Forfiet { get; set; }
        internal bool Gone { get; set; }

        internal uint KeyPresses { get; private set; }
        internal string Koth { get; set; }

        internal UpdateStatus ToUpdate { get; private set; }

        internal MatchPlayer(MultiplayerMatch match, UserData userData, uint socketId, IPAddress ipAddress)
        {
            this.Match = match;
            this.UserData = userData;

            this.SocketId = socketId;
            this.IPAddress = ipAddress;

            this._Speed = (int)userData.Speed;
            this._Accel = (int)userData.Accel;
            this._Jump = (int)userData.Jump;

            this._Hats = new Queue<MatchPlayerHat>();

            this.ToUpdate = UpdateStatus.None;
        }

        internal IReadOnlyCollection<MatchPlayerHat> Hats => this._Hats;

        internal void AddHat(uint id, Hat hat, Color color, bool spawned = true) => this.AddHat(new MatchPlayerHat(id, hat, color, spawned));
        internal void AddHat(MatchPlayerHat hat) => this._Hats.Enqueue(hat);

        internal MatchPlayerHat RemoveFirstHat()
        {
            if (this._Hats.TryDequeue(out MatchPlayerHat hat))
            {
                return hat;
            }

            return null;
        }

        internal double X
        {
            get => this._X;
            set
            {
                if (this._X != value)
                {
                    this._X = value;

                    this.ToUpdate |= UpdateStatus.X;
                }
            }
        }

        internal double Y
        {
            get => this._Y;
            set
            {
                if (this._Y != value)
                {
                    this._Y = value;

                    this.ToUpdate |= UpdateStatus.Y;
                }
            }
        }

        internal float VelX
        {
            get => this._VelX;
            set
            {
                if (this._VelX != value)
                {
                    this._VelX = value;

                    this.ToUpdate |= UpdateStatus.VelX;
                }
            }
        }

        internal float VelY
        {
            get => this._VelY;
            set
            {
                if (this._VelY != value)
                {
                    this._VelY = value;

                    this.ToUpdate |= UpdateStatus.VelY;
                }
            }
        }
        
        internal byte ScaleX
        {
            get => this._ScaleX;
            set
            {
                if (this._ScaleX != value)
                {
                    this._ScaleX = value;

                    this.ToUpdate |= UpdateStatus.ScaleX;
                }
            }
        }

        internal bool Space
        {
            get => this._Space;
            set
            {
                if (this._Space != value)
                {
                    this._Space = value;
                    this.KeyPresses++;

                    this.ToUpdate |= UpdateStatus.Space;
                }
            }
        }

        internal bool Left
        {
            get => this._Left;
            set
            {
                if (this._Left != value)
                {
                    this._Left = value;
                    this.KeyPresses++;

                    this.ToUpdate |= UpdateStatus.Left;
                }
            }
        }

        internal bool Right
        {
            get => this._Right;
            set
            {
                if (this._Right != value)
                {
                    this._Right = value;
                    this.KeyPresses++;

                    this.ToUpdate |= UpdateStatus.Right;
                }
            }
        }

        internal bool Down
        {
            get => this._Down;
            set
            {
                if (this._Down != value)
                {
                    this._Down = value;
                    this.KeyPresses++;

                    this.ToUpdate |= UpdateStatus.Down;
                }
            }
        }

        internal bool Up
        {
            get => this._Up;
            set
            {
                if (this._Up != value)
                {
                    this._Up = value;
                    this.KeyPresses++;

                    this.ToUpdate |= UpdateStatus.Up;
                }
            }
        }

        internal int Speed
        {
            get => this._Speed;
            set
            {
                if (this._Speed != value)
                {
                    this._Speed = value;

                    this.ToUpdate |= UpdateStatus.Speed;
                }
            }
        }

        internal int Accel
        {
            get => this._Accel;
            set
            {
                if (this._Accel != value)
                {
                    this._Accel = value;

                    this.ToUpdate |= UpdateStatus.Accel;
                }
            }
        }

        internal int Jump
        {
            get => this._Jump;
            set
            {
                if (this._Jump != value)
                {
                    this._Jump = value;

                    this.ToUpdate |= UpdateStatus.Jump;
                }
            }
        }

        internal float Rot
        {
            get => this._Rot;
            set
            {
                if (this._Rot != value)
                {
                    this._Rot = value;

                    this.ToUpdate |= UpdateStatus.Rot;
                }
            }
        }

        internal string Item
        {
            get => this._Item;
            set
            {
                this._Item = value;

                this.ToUpdate |= UpdateStatus.Item; //Always update item! ALWAYS
            }
        }

        internal uint Life
        {
            get => this._Life;
            set
            {
                if (this._Life != value)
                {
                    this._Life = value;

                    this.ToUpdate |= UpdateStatus.Life;
                }
            }
        }

        internal bool Hurt
        {
            get => this._Hurt;
            set
            {
                if (this._Hurt != value)
                {
                    this._Hurt = value;

                    this.ToUpdate |= UpdateStatus.Hurt;
                }
            }
        }

        internal uint Coins
        {
            get => this._Coins;
            set
            {
                if (this._Coins != value)
                {
                    this._Coins = value;

                    //this.ToUpdate |= UpdateStatus.Coins;
                }
            }
        }
        internal uint Dash
        {
            get => this._Dash;
            set
            {
                if (this._Dash != value)
                {
                    this._Dash = value;

                    //this.ToUpdate |= UpdateStatus.Dash;
                }
            }
        }

        internal string Team
        {
            get => this._Team;
            set
            {
                if (this._Team != value)
                {
                    this._Team = value;

                    this.ToUpdate |= UpdateStatus.Team;
                }
            }
        }

        internal bool GetUpdatePacket(out UpdateOutgoingPacket packet)
        {
            if (this.ToUpdate != UpdateStatus.None)
            {
                packet = new UpdateOutgoingPacket(this.ToUpdate, this);

                this.ToUpdate = UpdateStatus.None;

                return true;
            }

            packet = default;

            return false;
        }

        internal IReadOnlyDictionary<string, object> GetVars(params string[] vars) => this.GetVars(vars.ToHashSet());
        internal IReadOnlyDictionary<string, object> GetVars(HashSet<string> vars)
        {
            Dictionary<string, object> userVars = new();

            JsonUtils.GetVars(this.UserData, vars, userVars);
            JsonUtils.GetVars(this, vars, userVars);

            return userVars;
        }
    }
}
