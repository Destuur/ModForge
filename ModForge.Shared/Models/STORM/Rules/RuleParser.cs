using ModForge.Shared.Models.STORM.Operations;
using ModForge.Shared.Models.STORM.Selectors;
using System.Xml.Linq;

namespace ModForge.Shared.Models.STORM.Rules
{
	public static class RuleParser
	{
		public static List<Rule> ParseRules(XElement rulesElement, string category = null)
		{
			var rules = new List<Rule>();
			if (rulesElement == null) return rules;

			var nodes = rulesElement.Nodes().ToList();
			for (int i = 0; i < nodes.Count; i++)
			{
				if (nodes[i] is XElement ruleElem && ruleElem.Name == "rule")
				{
					string comment = null;
					if (i > 0 && nodes[i - 1] is XComment xcomment)
						comment = xcomment.Value.Trim();

					var rule = new Rule
					{
						Name = (string)ruleElem.Attribute("name"),
						Mode = (string)ruleElem.Attribute("mode"),
						Comment = comment,
						Selectors = SelectorParser.ParseSelectors(ruleElem.Element("selectors")),
						Operations = OperationParser.ParseOperations(ruleElem.Element("operations"), category)
					};
					rules.Add(rule);
				}
			}
			return rules;
		}
	}

}
