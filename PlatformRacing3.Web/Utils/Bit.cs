using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace PlatformRacing3.Web.Utils;

public struct Bit : IEquatable<Bit>, IXmlSerializable
{
	private int Value;

	public Bit(bool value)
	{
		this.Value = value ? 1 : 0;
	}

	public Bit(int value) : this(value != 0)
	{

	}

	public bool Equals(Bit other) => this.Value == other.Value;
	public override bool Equals(object obj) => obj is Bit && this.Equals((Bit)obj);
	public override int GetHashCode() => this.Value.GetHashCode();
	public override string ToString() => this.Value.ToString();

	public static implicit operator Bit(bool value) => new(value);
	public static implicit operator bool(Bit value) => value.Value == 1;

	public static explicit operator Bit(XElement value) => new(value.Value == "1");
	public static explicit operator Bit?(XElement value) => (bool?)value ?? false;

	public XmlSchema GetSchema() => null;

	public void ReadXml(XmlReader reader)
	{
		this.Value = reader.ReadElementContentAsInt();
	}

	public void WriteXml(XmlWriter writer)
	{
		writer.WriteValue(this.Value);
	}
}