﻿using Microsoft.AspNetCore.Components;
using ModForge.Shared.Adapter;
using ModForge.Shared.Models.Localizations;
using ModForge.Shared.Models.ModItems;

namespace ModForge.UI.Components.LocalizationComponents
{
	public partial class LocalizationComponent
	{
		//TODO: Toggle Modus und speichern von Localizations
		private bool editMode = true;
		private Dictionary<string, Dictionary<string, string>> name = new();
		private Dictionary<string, Dictionary<string, string>> description = new();
		private Dictionary<string, Dictionary<string, string>> loreDescription = new();

		private string NameKey = string.Empty;
		private string DescKey = string.Empty;
		private string LoreKey = string.Empty;

		[Parameter]
		public IModItem EditingModItem { get; set; }
		[Parameter]
		public IModItem OriginalModItem { get; set; }
		public List<string> SelectedLanguageCodes { get; set; } = new();

		public void AddLanguageKey(string key)
		{
			if (SelectedLanguageCodes.Contains(key))
			{
				SelectedLanguageCodes.Remove(key);
			}
			else
			{
				//SelectedLanguageCodes.Add(key);

				//OriginalModItem.Localization.Names.TryGetValue(key, out Dictionary<string, string> nameValue);
				//OriginalModItem.Localization.Descriptions.TryGetValue(key, out Dictionary<string, string> descValue);
				//OriginalModItem.Localization.LoreDescriptions.TryGetValue(key, out Dictionary<string, string> loreValue);

				//if (nameValue is not null)
				//{
				//	NameKey = nameValue.Keys.First();
				//}

				//if (descValue is not null)
				//{
				//	DescKey = descValue.Keys.First();
				//}

				//if (loreValue is not null)
				//{
				//	LoreKey = loreValue.Keys.First();
				//}

				//// Neue Dictionaries für diese Sprache hinzufügen, wenn sie nicht existieren
				//name.TryAdd(key, new Dictionary<string, string>() { { NameKey, "" } });
				//description.TryAdd(key, new Dictionary<string, string>() { { DescKey, "" } });
				//loreDescription.TryAdd(key, new Dictionary<string, string>() { { LoreKey, "" } });
			}

			StateHasChanged();
		}

		public void RemoveAllLanguages()
		{
			foreach (var languageKey in Languages.Map.Values)
			{

				SelectedLanguageCodes.Remove(languageKey);
			}
		}

		public void AddAllLanguages()
		{
			foreach (var languageKey in Languages.Map.Values)
			{
				AddLanguageKey(languageKey);
			}
		}
	}
}
