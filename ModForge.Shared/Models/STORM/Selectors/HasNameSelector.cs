using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM.Selectors
{
	public class HasNameSelector : SelectorBase
	{
		[XmlAttribute("name")]
		public string Name { get; set; }
	}
}
