using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM.Tasks
{
	public class Task
	{
		[XmlAttribute("name")]
		public string Name { get; set; }
		[XmlAttribute("class")]
		public string Class { get; set; }
		[XmlElement("source")]
		public List<Source> Sources { get; set; }
	}
}
