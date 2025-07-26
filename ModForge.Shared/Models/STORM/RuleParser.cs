using ModForge.Shared.Models.STORM.Rules;
using System.Xml;
using System.Xml.Linq;
using static ModForge.Shared.Models.STORM.OperationParser;

namespace ModForge.Shared.Models.STORM
{
	public static class RuleParser
	{
		public static List<Rule> ParseRules(XElement rulesElement)
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
						Operations = OperationParser.ParseOperations(ruleElem.Element("operations"))
					};

					foreach (var operation in rule.Operations)
					{
						if (operation is GenericOperation genericOperation)
						{
							if (genericOperation.Children.Count != 0)
							{

							}
						}
					}

					rules.Add(rule);
				}
			}
			return rules;
		}
	}

}
