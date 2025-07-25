using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM.Rules
{
	public class RuleContainer
	{
		[XmlElement("rule")]
		public List<Rule> RuleList { get; set; } = new();
	}
}
