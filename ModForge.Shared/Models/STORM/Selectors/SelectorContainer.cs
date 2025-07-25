using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM.Selectors
{
	public class SelectorContainer
	{
		[XmlElement("hasName", typeof(HasNameSelector))]
		[XmlElement("hasFaction", typeof(HasFactionSelector))]
		[XmlElement("hasSoulArchetype", typeof(HasSoulArchetypeSelector))]
		[XmlElement("hasSocialClass", typeof(HasSocialClassSelector))]
		[XmlElement("isHuman", typeof(IsHumanSelector))]
		[XmlElement("isMan", typeof(IsManSelector))]
		[XmlElement("isWoman", typeof(IsWomanSelector))]
		[XmlElement("isPlayer", typeof(IsPlayerSelector))]
		[XmlElement("isNotPlayer", typeof(IsNotPlayerSelector))]
		[XmlElement("isGameMode", typeof(IsGameModeSelector))]
		[XmlElement("isBackyardDog", typeof(IsBackyardDogSelector))]
		[XmlElement("isWarDog", typeof(IsWarDogSelector))]
		[XmlElement("isWolf", typeof(IsWolfSelector))]
		[XmlElement("isHorse", typeof(IsHorseSelector))]
		[XmlElement("isWarHorse", typeof(IsWarHorseSelector))]
		[XmlElement("isTravelHorse", typeof(IsTravelHorseSelector))]
		[XmlElement("isWorkHorse", typeof(IsWorkHorseSelector))]
		[XmlElement("isDraftHorse", typeof(IsDraftHorseSelector))]
		[XmlElement("and", typeof(AndSelector))]
		[XmlElement("or", typeof(OrSelector))]
		public List<ISelector> Selectors { get; set; } = new();
	}
}
