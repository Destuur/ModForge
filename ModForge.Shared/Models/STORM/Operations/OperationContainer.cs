using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM.Operations
{
	public class OperationContainer
	{
		[XmlElement("setAttribute", typeof(SetAttributeOperation))]
		[XmlElement("addPerk", typeof(AddPerkOperation))]
		public List<OperationBase> Operations { get; set; } = new();
	}
}
