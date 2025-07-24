using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM.Operations
{
	public class AddPerkOperation : OperationBase
	{
		[XmlAttribute("name")]
		public string Name { get; set; }
	}
}
