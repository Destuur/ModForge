namespace ModForge.Shared.Models.STORM.Selectors
{
	public class GenericSelector : ISelector
	{
		public string Name { get; set; }
		public Dictionary<string, string> Attributes { get; set; } = new();
		public List<GenericSelector> Children { get; set; } = new();

		public override string ToString()
		{
			return $"{Name} ({Attributes.Count} attrs, {Children.Count} children)";
		}
	}
}
