using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM.Selectors
{
	public class IsGameModeSelector : ISelector
	{
		[XmlAttribute("mode")]
		public string Mode { get; set; }
	}
}
