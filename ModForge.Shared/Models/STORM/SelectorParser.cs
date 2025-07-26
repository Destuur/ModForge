using System.Xml.Linq;

namespace ModForge.Shared.Models.STORM
{

	public static class SelectorParser
	{
		public static Dictionary<string, HashSet<string>> SelectorAttributes { get; set; } = new();

		public static List<GenericSelector> ParseSelectors(XElement selectorsElement)
		{
			var selectors = new List<GenericSelector>();
			if (selectorsElement == null) return selectors;

			foreach (var elem in selectorsElement.Elements())
			{
				selectors.Add(ParseGenericSelector(elem));
				#region Old Code
				//switch (elem.Name.LocalName)
				//{
				//	case "and":
				//		var andSelector = new AndSelector();
				//		andSelector.Selectors = ParseSelectors(elem); // Rekursion!
				//		selectors.Add(andSelector);
				//		break;

				//	case "or":
				//		var orSelector = new OrSelector();
				//		orSelector.Selectors = ParseSelectors(elem); // Rekursion!
				//		selectors.Add(orSelector);
				//		break;

				//	case "not":
				//		var notSelector = new NotSelector();
				//		notSelector.Selectors = ParseSelectors(elem); // Rekursion!
				//		selectors.Add(notSelector);
				//		break;

				//	case "hasName":
				//		selectors.Add(new HasNameSelector { Name = (string)elem.Attribute("name") });
				//		break;

				//	case "hasFaction":
				//		selectors.Add(new HasFactionSelector { Name = (string)elem.Attribute("name") });
				//		break;

				//	case "hasSoulArchetype":
				//		selectors.Add(new HasSoulArchetypeSelector { Name = (string)elem.Attribute("name") });
				//		break;

				//	case "hasSocialClass":
				//		selectors.Add(new HasSocialClassSelector { Name = (string)elem.Attribute("name") });
				//		break;

				//	case "hasVoice":
				//		selectors.Add(new HasVoiceSelector { Name = (string)elem.Attribute("name") });
				//		break;

				//	case "isBackyardDog":
				//		selectors.Add(new IsBackyardDogSelector());
				//		break;

				//	case "isWarDog":
				//		selectors.Add(new IsWarDogSelector());
				//		break;

				//	case "isWolf":
				//		selectors.Add(new IsWolfSelector());
				//		break;

				//	case "isHorse":
				//		selectors.Add(new IsHorseSelector());
				//		break;

				//	case "isWarHorse":
				//		selectors.Add(new IsWarHorseSelector());
				//		break;

				//	case "isTravelHorse":
				//		selectors.Add(new IsTravelHorseSelector());
				//		break;

				//	case "isWorkHorse":
				//		selectors.Add(new IsWorkHorseSelector());
				//		break;

				//	case "isDraftHorse":
				//		selectors.Add(new IsDraftHorseSelector());
				//		break;

				//	case "isHuman":
				//		selectors.Add(new IsHumanSelector());
				//		break;

				//	case "isNotPlayer":
				//		selectors.Add(new IsNotPlayerSelector());
				//		break;

				//	case "isPlayer":
				//		selectors.Add(new IsPlayerSelector());
				//		break;

				//	case "isValidOpenworldNpc":
				//		selectors.Add(new IsValidOpenworldNpcSelector());
				//		break;

				//	case "isMan":
				//		selectors.Add(new IsManSelector());
				//		break;

				//	case "isWoman":
				//		selectors.Add(new IsWomanSelector());
				//		break;

				//	default:
				//		Elements[elem] = elem.Name.LocalName;
				//		break;
				//}
				#endregion
			}
			return selectors;
		}

		public static GenericSelector ParseGenericSelector(XElement elem)
		{
			var selector = new GenericSelector
			{
				ElementName = elem.Name.LocalName
			};

			foreach (var attr in elem.Attributes())
			{
				selector.Attributes[attr.Name.LocalName] = attr.Value;
			}

			foreach (var child in elem.Elements())
			{
				selector.Children.Add(ParseGenericSelector(child));
			}

			RegisterAttributes(selector.ElementName, selector.Attributes.Keys, SelectorAttributes);
			return selector;
		}

		private static void RegisterAttributes(string name, IEnumerable<string> attributes, Dictionary<string, HashSet<string>> dict)
		{
			if (!dict.TryGetValue(name, out var attrSet))
			{
				attrSet = new HashSet<string>();
				dict[name] = attrSet;
			}

			foreach (var attr in attributes)
				attrSet.Add(attr);
		}
	}
}
