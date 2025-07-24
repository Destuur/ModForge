using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM.Operations
{
	public class SetAttributeOperation : OperationBase
	{
		[XmlAttribute("stat")]
		public string Stat { get; set; }

		[XmlAttribute("skill")]
		public string Skill { get; set; }

		[XmlAttribute("value")]
		public string Value { get; set; }
	}
}
