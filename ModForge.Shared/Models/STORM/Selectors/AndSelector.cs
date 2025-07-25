using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM.Selectors
{
	public class AndSelector : ISelector
	{
		[XmlElement("isPlayer", typeof(IsPlayerSelector))]
		[XmlElement("isGameMode", typeof(IsGameModeSelector))]
		public List<ISelector> Children { get; set; } = new();
	}

	public class OrSelector : ISelector
	{
		[XmlElement("isPlayer", typeof(IsPlayerSelector))]
		[XmlElement("isGameMode", typeof(IsGameModeSelector))]
		public List<ISelector> Children { get; set; } = new();
	}
}
