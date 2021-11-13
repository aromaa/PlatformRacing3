using System.Data.Common;
using System.Diagnostics;
using System.Net;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace PlatformRacing3.Common.Server;

public class ServerDetails : IXmlSerializable
{
	public uint Id { get; }
        
	public string Name { get; internal set; }
	public EndPoint EndPoint { get; internal set; }
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

		string ip = (string)reader["ip"];
		int port = (int)reader["port"];

		if (IPAddress.TryParse(ip, out IPAddress address))
		{
			this.EndPoint = new IPEndPoint(address, port);
		}
		else
		{
			this.EndPoint = new DnsEndPoint(ip, port);
		}

		this._Status = ServerManager.SERVER_STATUS_TIMEOUT_MESSAGE; //Lets show this by default
		this.LastStatusUpdate = new Stopwatch();
	}

	public string Status => this.LastStatusUpdate.Elapsed.TotalSeconds > ServerManager.SERVER_STATUS_TIMEOUT ? ServerManager.SERVER_STATUS_TIMEOUT_MESSAGE : this._Status;

	public string IP => this.EndPoint switch
	{
		IPEndPoint ip => ip.Address.ToString(),
		DnsEndPoint dns => dns.Host,

		_ => throw new NotSupportedException()
	};

	public int Port => this.EndPoint switch
	{
		IPEndPoint ip => ip.Port,
		DnsEndPoint dns => dns.Port,

		_ => throw new NotSupportedException()
	};

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