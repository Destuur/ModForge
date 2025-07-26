using ModForge.Shared.Models.STORM.Operations;
using ModForge.Shared.Models.STORM.Selectors;
using System.Xml.Linq;
using static ModForge.Shared.Models.STORM.OperationParser;
using Task = ModForge.Shared.Models.STORM.Tasks.Task;

namespace ModForge.Shared.Models.STORM
{
	public class StormParser
	{
		public Storm Parse(string filePath)
		{
			var doc = XDocument.Parse(filePath);
			var root = doc.Root;
			if (root == null || root.Name != "storm")
				throw new Exception("Keine gültige STORM-Datei (Root != <storm>)");

			var storm = new Storm();

			// Common
			var commonElement = root.Element("common");
			if (commonElement != null)
				storm.Common = ParseCommon(commonElement);

			// Tasks
			var tasksElement = root.Element("tasks");
			if (tasksElement != null)
				storm.Tasks = ParseTasks(tasksElement);

			// CustomSelectors
			var customSelectorsElement = root.Element("customSelectors");
			if (customSelectorsElement != null)
				storm.CustomSelectors = ParseCustomSelectors(customSelectorsElement);

			// CustomOperations
			var customOpsElement = root.Element("customOperations");
			if (customOpsElement != null)
				storm.CustomOperations = ParseCustomOperations(customOpsElement);

			// Rules
			var rulesElement = root.Element("rules");
			if (rulesElement != null)
				storm.Rules = RuleParser.ParseRules(rulesElement);

			return storm;
		}

		private Common ParseCommon(XElement commonElement)
		{
			var common = new Common();
			foreach (var sourceElem in commonElement.Elements("source"))
			{
				common.Sources.Add(new Source
				{
					Path = (string)sourceElem.Attribute("path")
				});
			}
			return common;
		}

		private List<Task> ParseTasks(XElement tasksElement)
		{
			var tasks = new List<Task>();
			var nodes = tasksElement.Nodes().ToList();

			for (int i = 0; i < nodes.Count; i++)
			{
				if (nodes[i] is XElement taskElem && taskElem.Name == "task")
				{
					string comment = null;
					if (i > 0 && nodes[i - 1] is XComment xcomment)
						comment = xcomment.Value.Trim();

					var task = new Task
					{
						Name = (string)taskElem.Attribute("name"),
						Comment = comment,
						Sources = taskElem.Elements("source").Select(s => new Source { Path = (string)s.Attribute("path") }).ToList()
					};

					tasks.Add(task);
				}
			}
			return tasks;
		}

		private List<CustomSelector> ParseCustomSelectors(XElement customSelectorsElement)
		{
			var selectors = new List<CustomSelector>();
			var nodes = customSelectorsElement.Nodes().ToList();

			for (int i = 0; i < nodes.Count; i++)
			{
				if (nodes[i] is XElement selElem)
				{
					string comment = null;
					if (i > 0 && nodes[i - 1] is XComment xcomment)
						comment = xcomment.Value.Trim();

					var sel = new CustomSelector
					{
						Name = (string)selElem.Attribute("name"),
						Comment = comment
						// Details ergänzen
					};

					selectors.Add(sel);
				}
			}
			return selectors;
		}

		private List<CustomOperation> ParseCustomOperations(XElement customOperationsElement)
		{
			var ops = new List<CustomOperation>();
			var nodes = customOperationsElement.Nodes().ToList();

			for (int i = 0; i < nodes.Count; i++)
			{
				if (nodes[i] is XElement opElem)
				{
					string comment = null;
					if (i > 0 && nodes[i - 1] is XComment xcomment)
						comment = xcomment.Value.Trim();

					var op = new CustomOperation
					{
						Name = (string)opElem.Attribute("name"),
						Mode = (string)opElem.Attribute("mode"),
						Comment = comment
						// Details ergänzen
					};

					ops.Add(op);
				}
			}
			return ops;
		}
	}

}
