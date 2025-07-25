using ModForge.Shared.Models.STORM.Rules;
using System.Xml;
using System.Xml.Linq;

namespace ModForge.Shared.Models.STORM
{
	public class StormParser
	{
		public List<Rule> ParseRules(XDocument doc)
		{
			var ruleElements = doc.Root.Element("rules")?.Elements();
			var rules = new List<Rule>();

			foreach (var element in ruleElements)
			{
				if (element.NodeType == XmlNodeType.Element && element is XElement ruleElement)
				{
					var comment = ruleElement.PreviousNode as XComment;

					var rule = new Rule
					{
						Name = ruleElement.Attribute("name")?.Value,
						Mode = ruleElement.Attribute("mode")?.Value,
						Comment = comment?.Value.Trim(),
						// hier könntest du Selectors und Operations mit weiteren Parsern auflösen
					};

					rules.Add(rule);
				}
			}

			return rules;
		}
	}
}
