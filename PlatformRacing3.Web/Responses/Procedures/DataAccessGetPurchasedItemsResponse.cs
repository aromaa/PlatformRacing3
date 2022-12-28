using System.Xml.Serialization;

namespace PlatformRacing3.Web.Responses.Procedures;

public class DataAccessGetPurchasedItemsResponse : DataAccessDataResponse<DataAccessGetPurchasedItemsResponse.PurchaseResponse>
{
	public DataAccessGetPurchasedItemsResponse()
	{
	}

	public DataAccessGetPurchasedItemsResponse(List<string> items)
	{
		this.Rows = items.Select(i => new PurchaseResponse(i)).ToList();
	}

	public class PurchaseResponse
	{
		[XmlElement("item")]
		public string Item { get; set; }

		public PurchaseResponse()
		{

		}

		public PurchaseResponse(string item)
		{
			this.Item = item;
		}
	}
}
