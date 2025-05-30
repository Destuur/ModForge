using KCD2.ModForge.Shared.Models.Attributes;
using KCD2.ModForge.Shared.Mods;
using System.Text.Json.Serialization;

namespace KCD2.ModForge.Shared.Models.Localizations
{
	public class Localization
	{
		public Localization()
		{

		}

		public Localization(Dictionary<string, string> names, Dictionary<string, string> defaultDescription, Dictionary<string, string> descriptions, Dictionary<string, string> loreDescriptions)
		{
			Names = names;
			DefaultDescriptions = defaultDescription;
			Descriptions = descriptions;
			LoreDescriptions = loreDescriptions;
		}

		public Dictionary<string, string>? Names { get; } = new();
		public Dictionary<string, string>? DefaultDescriptions { get; } = new();
		public Dictionary<string, string>? Descriptions { get; } = new();
		public Dictionary<string, string>? LoreDescriptions { get; } = new();


		public string? GetName(string language)
		{
			return Names.TryGetValue(language, out var value) ? value : null;
		}

		public string? GetDefault(string language)
		{
			return DefaultDescriptions.TryGetValue(language, out var value) ? value : null;
		}

		public string? GetDescription(string language)
		{
			return Descriptions.TryGetValue(language, out var value) ? value : null;
		}

		public string? GetLoreDescription(string language)
		{
			return LoreDescriptions.TryGetValue(language, out var value) ? value : null;
		}

		public void SetName(string language, string text)
		{
			Names[language] = text;
		}

		public void SetDefault(string language, string text)
		{
			DefaultDescriptions[language] = text;
		}

		public void SetDescription(string language, string text)
		{
			Descriptions[language] = text;
		}

		public void SetLoreDescription(string language, string text)
		{
			LoreDescriptions[language] = text;
		}
	}
}
