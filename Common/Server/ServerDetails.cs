using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Platform_Racing_3_Common.Server
{
    public class ServerDetails : IXmlSerializable
    {
        public uint Id { get; }
        
        public string Name { get; internal set; }
        public IPEndPoint EndPoint { get; internal set; }
        private string _Status;

        /// <summary>
        /// If last update is more than server status timeout we will show server status timeout message instead
        /// </summary>
        internal Stopwatch LastStatusUpdate { get; set; }

        private ServerDetails()
        {

        }

        public ServerDetails(DbDataReader reader)
        {
            this.Id = (uint)(int)reader["id"];
            this.Name = (string)reader["name"];
            this.EndPoint = new IPEndPoint((IPAddress)reader["ip"], (int)reader["port"]);

            this._Status = ServerManager.SERVER_STATUS_TIMEOUT_MESSAGE; //Lets show this by default
            this.LastStatusUpdate = new Stopwatch();
        }

        public string Status => this.LastStatusUpdate.Elapsed.TotalSeconds > ServerManager.SERVER_STATUS_TIMEOUT ? ServerManager.SERVER_STATUS_TIMEOUT_MESSAGE : this._Status;
            
        public IPAddress IPAddress
        {
            get => this.EndPoint.Address;
            set => this.EndPoint.Address = value;
        }
        
        public string IP
        {
            get => this.EndPoint.Address.ToString();
            set => this.EndPoint.Address = IPAddress.Parse(value);
        }
        
        public ushort Port
        {
            get => (ushort)this.EndPoint.Port;
            set => this.EndPoint.Port = value;
        }

        internal void SetStatus(string status)
        {
            this._Status = status;
            this.LastStatusUpdate.Restart();
        }

        public XmlSchema GetSchema() => null;

        public void ReadXml(XmlReader reader) => throw new NotSupportedException();
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("server_name", this.Name);
            writer.WriteElementString("status", this.Status);
            writer.WriteElementString("address", this.IP);
            writer.WriteElementString("port", this.Port.ToString());
        }
    }
}
