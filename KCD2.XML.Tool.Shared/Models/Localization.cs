namespace KCD2.XML.Tool.Shared.Models
{
	public class Localization : IModItem
	{
		public string Id { get; } = string.Empty;
		public string Path { get; private set; } = string.Empty;
		public Dictionary<string, string> Attributes { get; } = new();
	}
}
