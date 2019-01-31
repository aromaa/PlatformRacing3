using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Platform_Racing_3_Common.Campaign;
using static Platform_Racing_3_Web.Responses.Procedures.DataAccessGetMyFriendsFastestRunsResponse;

namespace Platform_Racing_3_Web.Responses.Procedures
{
    public class DataAccessGetMyFriendsFastestRunsResponse : DataAccessDataResponse<UserCampaignRun>
    {
        public DataAccessGetMyFriendsFastestRunsResponse()
        {
            this.Rows = new List<UserCampaignRun>(3);
        }

        internal void AddRun(uint userId, int time, CampaignRun run)
        {
            this.Rows.Add(new UserCampaignRun(userId, time, run));
        }

        public class UserCampaignRun
        {
            [XmlElement("user_id")]
            public uint UserId { get; set; }
            [XmlElement("best_time_ms")]
            public int Time { get; set; }
            private CampaignRun Run { get; }

            [XmlElement("name")]
            public string Username
            {
                get => this.Run.Username;
                set
                {

                }
            }

            [XmlElement("hat")]
            public uint Hat
            {
                get => (uint)this.Run.Hat;
                set
                {

                }
            }

            [XmlElement("head")]
            public uint Head
            {
                get => (uint)this.Run.Head;
                set
                {

                }
            }

            [XmlElement("body")]
            public uint Body
            {
                get => (uint)this.Run.Body;
                set
                {

                }
            }

            [XmlElement("feet")]
            public uint Feet
            {
                get => (uint)this.Run.Feet;
                set
                {

                }
            }

            [XmlElement("hat_color")]
            public int HatColor
            {
                get => this.Run.HatColor.ToArgb();
                set
                {

                }
            }

            [XmlElement("head_color")]
            public int HeadColor
            {
                get => this.Run.HeadColor.ToArgb();
                set
                {

                }
            }

            [XmlElement("body_color")]
            public int BodyColor
            {
                get => this.Run.BodyColor.ToArgb();
                set
                {

                }
            }

            [XmlElement("feet_color")]
            public int FeetColor
            {
                get => this.Run.FeetColor.ToArgb();
                set
                {

                }
            }

            public UserCampaignRun()
            {

            }

            public UserCampaignRun(uint userId, int time, CampaignRun run)
            {
                this.UserId = userId;
                this.Time = time;
                this.Run = run;
            }
        }
    }
}
