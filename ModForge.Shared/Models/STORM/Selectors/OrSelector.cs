namespace ModForge.Shared.Models.STORM.Selectors
{
	public class OrSelector : ISelector
	{
		public List<ISelector> Selectors { get; set; } = new();
	}
}
