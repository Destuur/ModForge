using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM.Tasks
{
	public class TaskContainer
	{
		[XmlElement("task")]
		public List<Task> TaskItems { get; set; }
	}
}
