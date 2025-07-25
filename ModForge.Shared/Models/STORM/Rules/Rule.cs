using ModForge.Shared.Models.STORM.Operations;
using ModForge.Shared.Models.STORM.Selectors;
using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM.Rules
{
	public class Rule
	{
		public string Name { get; set; }
		public string Mode { get; set; }
		public string Comment { get; set; }
		public SelectorContainer Selectors { get; set; }
		public OperationContainer Operations { get; set; }
	}
}
