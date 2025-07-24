using ModForge.Shared.Models.STORM.Operations;
using ModForge.Shared.Models.STORM.Selectors;
using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM.Rules
{
	public class Rule
	{
		[XmlAttribute("name")]
		public string Name { get; set; }

		[XmlElement("selectors")]
		public SelectorContainer Selectors { get; set; }

		[XmlElement("operations")]
		public OperationContainer Operations { get; set; }
	}
}
