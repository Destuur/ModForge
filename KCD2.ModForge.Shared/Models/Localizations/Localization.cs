namespace KCD2.ModForge.Shared.Models.Localizations
{
	public class Localization
	{
		public Localization()
		{

		}

		public Localization(Dictionary<string, string> names, Dictionary<string, string> descriptions, Dictionary<string, string> loreDescriptions)
		{
			Names = names;
			Descriptions = descriptions;
			LoreDescriptions = loreDescriptions;
		}

		public Dictionary<string, string>? Names { get; set; } = new();
		public Dictionary<string, string>? Descriptions { get; set; } = new();
		public Dictionary<string, string>? LoreDescriptions { get; set; } = new();

		public Localization DeepClone()
		{
			return new Localization(
				new Dictionary<string, string>(Names),
				new Dictionary<string, string>(Descriptions),
				new Dictionary<string, string>(LoreDescriptions));
		}

		public string? GetName(string language)
		{
			return Names.TryGetValue(language, out var value) ? value : null;
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
