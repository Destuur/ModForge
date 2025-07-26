using ModForge.Shared.Models.Abstractions;
using ModForge.Shared.Models.STORM.Operations;
using ModForge.Shared.Models.STORM.Rules;
using ModForge.Shared.Models.STORM.Selectors;
using Task = ModForge.Shared.Models.STORM.Tasks.Task;

namespace ModForge.Shared.Models.STORM
{
	public class StormDto
	{
		public IDataPoint DataPoint { get; set; }
		public string Id { get; set; }
		public string Category { get; set; }
		public Common Common { get; set; }
		public List<Rule> Rules { get; set; } = new();
		public List<Task> Tasks { get; set; } = new();
		public List<CustomSelector> CustomSelectors { get; set; } = new();
		public List<CustomOperation> CustomOperations { get; set; } = new();
	}
}
