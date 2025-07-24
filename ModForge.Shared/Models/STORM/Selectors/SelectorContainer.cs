using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM.Selectors
{
	public class SelectorContainer
	{
		[XmlElement("hasName", typeof(HasNameSelector))]
		[XmlElement("isPlayer", typeof(IsPlayerSelector))]
		[XmlElement("isGameMode", typeof(IsGameModeSelector))]
		[XmlElement("and", typeof(AndSelector))]
		public List<SelectorBase> Selectors { get; set; } = new();
	}
}
