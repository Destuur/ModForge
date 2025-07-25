using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM.Selectors
{
	public class IsGameModeSelector : ISelector
	{
		public string Mode { get; set; }
	}
}
