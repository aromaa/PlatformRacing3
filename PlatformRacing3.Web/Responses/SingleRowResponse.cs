using System.Xml.Serialization;

namespace PlatformRacing3.Web.Responses;

public class SingleRowResponse<T>
{
	[XmlElement("Row")]
	public T Row;

	public SingleRowResponse()
	{

	}

	public SingleRowResponse(T row)
	{
		this.Row = row;
	}
}