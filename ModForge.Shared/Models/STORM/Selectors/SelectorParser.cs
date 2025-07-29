using System.Xml.Linq;

namespace ModForge.Shared.Models.STORM.Selectors
{

	public static class SelectorParser
	{
		public static Dictionary<string, Dictionary<string, HashSet<string>>> SelectorAttributes { get; set; } = new();

		public static List<GenericSelector> ParseSelectors(XElement selectorsElement)
		{
			var selectors = new List<GenericSelector>();
			if (selectorsElement == null) return selectors;

			foreach (var elem in selectorsElement.Elements())
			{
				selectors.Add(ParseGenericSelector(elem));
			}
			return selectors;
		}

		public static GenericSelector ParseGenericSelector(XElement elem)
		{
			var selector = new GenericSelector
			{
				Name = elem.Name.LocalName
			};

			foreach (var attr in elem.Attributes())
			{
				selector.Attributes[attr.Name.LocalName] = attr.Value;
			}

			foreach (var child in elem.Elements())
			{
				selector.Children.Add(ParseGenericSelector(child));
			}

			RegisterAttributes(selector.Name, selector.Attributes);
			return selector;
		}

		private static void RegisterAttributes(string selectorName, Dictionary<string, string> attributes)
		{
			if (!SelectorAttributes.TryGetValue(selectorName, out var attributeDict))
			{
				attributeDict = new Dictionary<string, HashSet<string>>();
				SelectorAttributes[selectorName] = attributeDict;
			}

			foreach (var kvp in attributes)
			{
				if (!attributeDict.TryGetValue(kvp.Key, out var values))
				{
					values = new HashSet<string>();
					attributeDict[kvp.Key] = values;
				}

				values.Add(kvp.Value);
			}
		}
	}
}
