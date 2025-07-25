using ModForge.Shared.Models.STORM.Operations;
using System.Xml.Linq;

namespace ModForge.Shared.Models.STORM
{
	public static partial class OperationParser
	{
		public static Dictionary<string, HashSet<string>> OperationAttributes { get; set; } = new();
		public static Dictionary<XElement, string> Elements { get; set; } = new();

		public static List<IOperation> ParseOperations(XElement operationsElement)
		{
			var operations = new List<IOperation>();
			if (operationsElement == null) return operations;

			foreach (var elem in operationsElement.Elements())
			{
				operations.Add(ParseGenericOperation(elem));
				#region Old Code
				//switch (elem.Name.LocalName)
				//{
				//	case "modAttribute":
				//		operations.Add(new ModAttributeOperation()
				//		{
				//			Stat = (string)elem.Attribute("stat"),
				//			MinMod = (double?)elem.Attribute("minMod"),
				//			MaxMod = (double?)elem.Attribute("maxMod")
				//		});
				//		break;

				//	case "setAttribute":
				//		operations.Add(new SetAttributeOperation()
				//		{
				//			Stat = (string)elem.Attribute("stat"),
				//			Skill = (string)elem.Attribute("skill"),
				//			Value = (double?)elem.Attribute("value"),
				//			MinValue = (double?)elem.Attribute("minValue"),
				//			MaxValue = (double?)elem.Attribute("maxValue"),
				//			ScaleWith = (string)elem.Attribute("scaleWith")
				//		});
				//		break;

				//	case "addPerk":
				//		operations.Add(new AddPerkOperation()
				//		{
				//			Name = (string)elem.Attribute("name"),
				//		});
				//		break;

				//	case "removePerk":
				//		operations.Add(new RemovePerkOperation()
				//		{
				//			Name = (string)elem.Attribute("name"),
				//		});
				//		break;

				//	case "addRole":
				//		operations.Add(new AddRoleOperation()
				//		{
				//			Name = (string)elem.Attribute("name"),
				//		});
				//		break;

				//	case "removeRole":
				//		operations.Add(new RemoveRoleOperation()
				//		{
				//			Name = (string)elem.Attribute("name"),
				//		});
				//		break;

				//	case "addContext":
				//		operations.Add(new AddContextOperation()
				//		{
				//			Name = (string)elem.Attribute("name"),
				//		});
				//		break;

				//	case "addMetarole":
				//		operations.Add(new AddMetaroleOperation()
				//		{
				//			Name = (string)elem.Attribute("name"),
				//		});
				//		break;

				//	case "addInventory":
				//		operations.Add(new AddInventoryOperation()
				//		{
				//			Preset = (string)elem.Attribute("preset"),
				//		});
				//		break;

				//	case "setBody":
				//		operations.Add(new SetBodyOperation()
				//		{
				//			Name = (string)elem.Attribute("name"),
				//		});
				//		break;

				//	case "setHead":
				//		operations.Add(new SetHeadOperation()
				//		{
				//			Name = (string)elem.Attribute("name"),
				//		});
				//		break;

				//	case "setUiName":
				//		operations.Add(new SetUiNameOperation()
				//		{
				//			Name = (string)elem.Attribute("name"),
				//		});
				//		break;

				//	case "setInventory":
				//		operations.Add(new SetInventoryOperation()
				//		{
				//			Name = (string)elem.Attribute("name"),
				//		});
				//		break;

				//	case "setReputation":
				//		operations.Add(new SetReputationOperation()
				//		{
				//			Value = (double?)elem.Attribute("value"),
				//		});
				//		break;

				//	default:
				//		Elements[elem] = $"Unknown operation type: {elem.Name.LocalName}";
				//		break;
				//}
				#endregion
			}
			return operations;
		}

		public static GenericOperation ParseGenericOperation(XElement elem)
		{
			var operation = new GenericOperation
			{
				ElementName = elem.Name.LocalName
			};

			foreach (var attr in elem.Attributes())
			{
				operation.Attributes[attr.Name.LocalName] = attr.Value;
			}

			foreach (var child in elem.Elements())
			{
				operation.Children.Add(ParseGenericOperation(child));
			}

			RegisterAttributes(operation.ElementName, operation.Attributes.Keys, OperationAttributes);
			return operation;
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
