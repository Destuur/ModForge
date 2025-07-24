using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM.Selectors
{
	public class IsGameModeSelector : SelectorBase
	{
		[XmlAttribute("mode")]
		public string Mode { get; set; }
	}
}
