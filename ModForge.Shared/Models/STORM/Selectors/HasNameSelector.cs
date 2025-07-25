using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM.Selectors
{
	public class HasNameSelector : ISelector
	{
		[XmlAttribute("name")]
		public string Name { get; set; }
	}

	public class HasFactionSelector : ISelector
	{
		[XmlAttribute("name")]
		public string Name { get; set; }
	}

	public class HasSoulArchetypeSelector : ISelector
	{
		[XmlAttribute("name")]
		public string Name { get; set; }
	}

	public class HasSocialClassSelector : ISelector
	{
		[XmlAttribute("name")]
		public string Name { get; set; }
	}

	public class IsBackyardDogSelector : ISelector
	{

	}

	public class IsWarDogSelector : ISelector
	{

	}

	public class IsWolfSelector : ISelector
	{

	}

	public class IsHorseSelector : ISelector
	{

	}

	public class IsWarHorseSelector : ISelector
	{

	}

	public class IsTravelHorseSelector : ISelector
	{

	}

	public class IsWorkHorseSelector : ISelector
	{

	}

	public class IsDraftHorseSelector : ISelector
	{

	}

	public class IsHumanSelector : ISelector
	{

	}

	public class IsNotPlayerSelector : ISelector
	{

	}

	public class IsManSelector : ISelector
	{

	}

	public class IsWomanSelector : ISelector
	{

	}
}
