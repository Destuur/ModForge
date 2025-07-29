using ModForge.Shared.Models.STORM.Operations;
using ModForge.Shared.Models.STORM.Selectors;

namespace ModForge.Shared.Models.STORM.Rules
{
	public class Rule
	{
		public string Name { get; set; }
		public string Mode { get; set; }
		public string Comment { get; set; }
		public string Category { get; set; }
		public List<GenericSelector> Selectors { get; set; } = new();
		public List<GenericOperation> Operations { get; set; } = new();
	}
}
