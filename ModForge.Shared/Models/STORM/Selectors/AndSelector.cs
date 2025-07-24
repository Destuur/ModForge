using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM.Selectors
{
	public class AndSelector : SelectorBase
	{
		[XmlElement("isPlayer", typeof(IsPlayerSelector))]
		[XmlElement("isGameMode", typeof(IsGameModeSelector))]
		public List<SelectorBase> Children { get; set; } = new();
	}
}
