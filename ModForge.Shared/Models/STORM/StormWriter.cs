using ModForge.Shared.Models.STORM.Operations;
using ModForge.Shared.Models.STORM.Rules;
using ModForge.Shared.Models.STORM.Selectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ModForge.Shared.Models.STORM
{
	public class StormWriter
	{
		public void WriteEntryPointStormFile(string outputPath, List<Rule> rules, string id)
		{
			var tasksElement = new XElement("tasks");

			foreach (var rule in rules.Where(r => !string.IsNullOrEmpty(r.Category)))
			{
				var task = new XElement("task",
					new XAttribute("name", rule.Category),
					new XAttribute("class", rule.Category),
					new XElement("source", new XAttribute("path", $"{rule.Name}.xml"))
				);

				tasksElement.Add(task);
			}

			var document = new XDocument(
				new XDeclaration("1.0", "utf-8", null),
				new XElement("storm", tasksElement)
			);

			var fullPath = Path.Combine(outputPath, $"storm__{id}.xml");
			Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
			document.Save(fullPath);
		}

		public void WriteRuleFilesPerRule(string outputPath, List<Rule> rules)
		{
			foreach (var rule in rules)
			{
				var rulesElement = new XElement("rules");

				var ruleElement = new XElement("rule",
					new XAttribute("name", rule.Name));

				var selectorsElement = SelectorWriter.Write(rule.Selectors);
				var operationsElement = OperationWriter.Write(rule.Operations);

				ruleElement.Add(selectorsElement);
				ruleElement.Add(operationsElement);
				rulesElement.Add(ruleElement);

				var doc = new XDocument(
					new XDeclaration("1.0", "utf-8", null),
					new XElement("storm", rulesElement)
				);

				var filePath = Path.Combine(outputPath, $"{rule.Name}.xml");
				Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
				doc.Save(filePath);
			}
		}
	}

	public static class SelectorWriter
	{
		public static XElement Write(List<GenericSelector> selectors)
		{
			var root = new XElement("selectors");
			foreach (var selector in selectors)
			{
				root.Add(WriteSelector(selector));
			}
			return root;
		}

		private static XElement WriteSelector(GenericSelector selector)
		{
			var element = new XElement(selector.Name);

			foreach (var attr in selector.Attributes)
			{
				element.SetAttributeValue(attr.Key, attr.Value);
			}

			if (selector.Children.Any())
			{
				foreach (var child in selector.Children)
				{
					element.Add(WriteSelector(child));
				}
			}

			return element;
		}
	}

	public static class OperationWriter
	{
		public static XElement Write(List<GenericOperation> operations)
		{
			var root = new XElement("operations");
			foreach (var selector in operations)
			{
				root.Add(WriteOperation(selector));
			}
			return root;
		}

		private static XElement WriteOperation(GenericOperation selector)
		{
			var element = new XElement(selector.Name);

			foreach (var attr in selector.Attributes)
			{
				element.SetAttributeValue(attr.Key, attr.Value);
			}

			if (selector.Children.Any())
			{
				foreach (var child in selector.Children)
				{
					element.Add(WriteOperation(child));
				}
			}

			return element;
		}
	}
}
