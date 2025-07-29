using System.Xml.Linq;

namespace ModForge.Shared.Models.STORM.Operations
{
	public static class OperationParser
	{
		public static Dictionary<string, OperationCategory> Categories { get; set; } = new();

		public static List<GenericOperation> ParseOperations(XElement operationsElement, string category = null)
		{
			var operations = new List<GenericOperation>();
			if (operationsElement == null) return operations;

			foreach (var elem in operationsElement.Elements())
			{
				operations.Add(ParseGenericOperation(elem, category));
			}
			return operations;
		}

		public static GenericOperation ParseGenericOperation(XElement elem, string category = null)
		{
			var operation = new GenericOperation
			{
				Name = elem.Name.LocalName
			};

			foreach (var attr in elem.Attributes())
			{
				operation.Attributes[attr.Name.LocalName] = attr.Value;
			}

			foreach (var child in elem.Elements())
			{
				operation.Children.Add(ParseGenericOperation(child));
			}

			RegisterAttributes(category, operation.Name, operation.Attributes, Categories);
			return operation;
		}

		public static void RegisterAttributes(string category, string operationName, Dictionary<string, string> attributes, Dictionary<string, OperationCategory> categoryRegistry)
		{
			if (string.IsNullOrEmpty(category))
				return;

			if (!categoryRegistry.TryGetValue(category, out var def))
			{
				def = new OperationCategory { Name = category };
				categoryRegistry[category] = def;
			}

			def.OperationTypes.Add(operationName);

			if (!def.OperationAttributes.TryGetValue(operationName, out var attrMap))
			{
				attrMap = new Dictionary<string, HashSet<string>>();
				def.OperationAttributes[operationName] = attrMap;
			}

			foreach (var attr in attributes)
			{
				if (!attrMap.TryGetValue(attr.Key, out var valueSet))
				{
					valueSet = new HashSet<string>();
					attrMap[attr.Key] = valueSet;
				}
				valueSet.Add(attr.Value);
			}
		}

	}

	public class OperationCategory
	{
		public string Name { get; set; }
		public HashSet<string> OperationTypes { get; set; } = new();
		public Dictionary<string, Dictionary<string, HashSet<string>>> OperationAttributes { get; set; } = new();
	}
}
