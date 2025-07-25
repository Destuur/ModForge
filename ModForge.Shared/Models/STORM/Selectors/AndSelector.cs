using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM.Selectors
{
	public class AndSelector : ISelector
	{
		public List<ISelector> Selectors { get; set; } = new();
	}
}
