namespace KCD2.ModForge.Shared.Models.Localizations
{
	public class Localization
	{
		public Localization()
		{

		}

		public Localization(Dictionary<string, Dictionary<string, string>> names, Dictionary<string, Dictionary<string, string>> descriptions, Dictionary<string, Dictionary<string, string>> loreDescriptions)
		{
			Names = names;
			Descriptions = descriptions;
			LoreDescriptions = loreDescriptions;
		}

		public Dictionary<string, Dictionary<string, string>>? Names { get; set; } = new();
		public Dictionary<string, Dictionary<string, string>>? Descriptions { get; set; } = new();
		public Dictionary<string, Dictionary<string, string>>? LoreDescriptions { get; set; } = new();

		public Localization DeepClone()
		{
			return new Localization(
				Names.ToDictionary(
					outer => outer.Key,
					outer => outer.Value.ToDictionary(inner => inner.Key, inner => inner.Value)
				),
				Descriptions.ToDictionary(
					outer => outer.Key,
					outer => outer.Value.ToDictionary(inner => inner.Key, inner => inner.Value)
				),
				LoreDescriptions.ToDictionary(
					outer => outer.Key,
					outer => outer.Value.ToDictionary(inner => inner.Key, inner => inner.Value)
				)
			);
		}


		public string? GetName(string language)
		{
			return Names.TryGetValue(language, out var value) ? value.Values.First() : null;
		}

		public string? GetNameKey(string language)
		{
			return Names.TryGetValue(language, out var value) ? value.Keys.First() : null;
		}

		public string? GetDescription(string language)
		{
			return Descriptions.TryGetValue(language, out var value) ? value.Values.First() : null;
		}

		public string? GetDescriptionKey(string language)
		{
			return Descriptions.TryGetValue(language, out var value) ? value.Keys.First() : null;
		}

		public string? GetLoreDescription(string language)
		{
			return LoreDescriptions.TryGetValue(language, out var value) ? value.Values.First() : null;
		}

		public string? GetLoreDescriptionKey(string language)
		{
			return LoreDescriptions.TryGetValue(language, out var value) ? value.Keys.First() : null;
		}

		public void SetName(string language, string key, string text)
		{
			if (!Names.ContainsKey(language))
				Names[language] = new Dictionary<string, string>();

			Names[language][key] = text;
		}

		public void SetDescription(string language, string key, string text)
		{
			if (!Descriptions.ContainsKey(language))
				Descriptions[language] = new Dictionary<string, string>();

			Descriptions[language][key] = text;
		}

		public void SetLoreDescription(string language, string key, string text)
		{
			if (!LoreDescriptions.ContainsKey(language))
				LoreDescriptions[language] = new Dictionary<string, string>();

			LoreDescriptions[language][key] = text;
		}
	}
}
