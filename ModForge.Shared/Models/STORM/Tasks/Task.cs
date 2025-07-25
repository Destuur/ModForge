using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM.Tasks
{
	public class Task
	{
		public string Name { get; set; }
		public string Class { get; set; }
		public string Comment { get; set; }
		public List<Source> Sources { get; set; } = new();
	}
}
