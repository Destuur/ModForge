using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM.Selectors
{
	[XmlInclude(typeof(HasNameSelector))]
	[XmlInclude(typeof(HasFactionSelector))]
	[XmlInclude(typeof(HasSoulArchetypeSelector))]
	[XmlInclude(typeof(HasSocialClassSelector))]
	[XmlInclude(typeof(IsHumanSelector))]
	[XmlInclude(typeof(IsManSelector))]
	[XmlInclude(typeof(IsWomanSelector))]
	[XmlInclude(typeof(IsPlayerSelector))]
	[XmlInclude(typeof(IsNotPlayerSelector))]
	[XmlInclude(typeof(IsGameModeSelector))]
	[XmlInclude(typeof(IsBackyardDogSelector))]
	[XmlInclude(typeof(IsWarDogSelector))]
	[XmlInclude(typeof(IsWolfSelector))]
	[XmlInclude(typeof(IsHorseSelector))]
	[XmlInclude(typeof(IsWarHorseSelector))]
	[XmlInclude(typeof(IsTravelHorseSelector))]
	[XmlInclude(typeof(IsWorkHorseSelector))]
	[XmlInclude(typeof(IsDraftHorseSelector))]
	[XmlInclude(typeof(AndSelector))]
	[XmlInclude(typeof(OrSelector))]
	public interface ISelector { }
}
