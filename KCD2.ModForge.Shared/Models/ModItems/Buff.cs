using KCD2.ModForge.Shared.Mods;

namespace KCD2.ModForge.Shared.Models.ModItems
{
	public class Buff : IModItem
	{
		public Buff(string id)
		{
			Id = id;
		}

		public string Id { get; } = string.Empty;
		public string Path { get; private set; } = string.Empty;
		public Dictionary<string, string> Attributes { get; } = new();
		public IEnumerable<IModItem>? Localizations { get; private set; }
	}
}
