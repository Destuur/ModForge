using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM.Selectors
{
	public class AndSelector : ISelector, IConditionalSelector
	{
		public List<ISelector> Selectors { get; set; } = new();
	}
}
