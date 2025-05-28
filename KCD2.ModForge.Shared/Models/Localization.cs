using KCD2.ModForge.Shared.Models.Attributes;
using KCD2.ModForge.Shared.Mods;

namespace KCD2.ModForge.Shared.Models
{
	public class Localization
	{
		public Dictionary<string, string> Names { get; } = new();
		public Dictionary<string, string> Descriptions { get; } = new();
		public Dictionary<string, string> LoreDescriptions { get; } = new();

		public string? GetName(string language) =>
			Names.TryGetValue(language, out var value) ? value : null;

		public string? GetDescription(string language) =>
			Descriptions.TryGetValue(language, out var value) ? value : null;

		public string? GetLoreDescription(string language) =>
			LoreDescriptions.TryGetValue(language, out var value) ? value : null;
	}

	public class LocalizationEntry
	{
		public string Key { get; set; } = string.Empty;
		public string Text { get; set; } = string.Empty;
	}
}
