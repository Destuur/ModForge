using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM
{
	public class Common
	{
		[XmlElement("source")]
		public List<Source> Sources { get; set; }
	}
}
