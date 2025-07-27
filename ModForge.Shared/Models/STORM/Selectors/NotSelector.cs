namespace ModForge.Shared.Models.STORM.Selectors
{
	public class NotSelector : ISelector, IConditionalSelector
	{
		public List<ISelector> Selectors { get; set; } = new();
	}
}
