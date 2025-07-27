namespace ModForge.Shared.Models.STORM.Selectors
{
	public class OrSelector : ISelector, IConditionalSelector
	{
		public List<ISelector> Selectors { get; set; } = new();
	}
}
