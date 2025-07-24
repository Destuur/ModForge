using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM.Selectors
{
	[XmlInclude(typeof(HasNameSelector))]
	[XmlInclude(typeof(IsPlayerSelector))]
	[XmlInclude(typeof(IsGameModeSelector))]
	[XmlInclude(typeof(AndSelector))]
	public abstract class SelectorBase { }
}
