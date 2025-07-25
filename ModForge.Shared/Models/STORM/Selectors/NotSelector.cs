namespace ModForge.Shared.Models.STORM.Selectors
{
	public class NotSelector : ISelector
	{
		public List<ISelector> Selectors { get; set; } = new();
	}
}
