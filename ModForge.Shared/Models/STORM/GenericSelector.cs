using ModForge.Shared.Models.STORM.Selectors;

namespace ModForge.Shared.Models.STORM
{
	public class GenericSelector : ISelector
	{
		public string ElementName { get; set; }
		public Dictionary<string, string> Attributes { get; set; } = new();
		public List<ISelector> Children { get; set; } = new();

		public override string ToString()
		{
			return $"{ElementName} ({Attributes.Count} attrs, {Children.Count} children)";
		}
	}
}
