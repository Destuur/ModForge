using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM.Rules
{
	public class RuleCollection
	{
		[XmlElement("rule")]
		public List<Rule> RuleList { get; set; } = new();
	}
}
