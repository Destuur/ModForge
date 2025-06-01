using KCD2.ModForge.Shared.Adapter;
using KCD2.ModForge.Shared.Models.ModItems;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCD2.ModForge.UI.Components.LocalizationComponents
{
	public partial class LocalizationComponent
	{
		//TODO: Toggle Modus und speichern von Localizations
		private bool editMode = true;
		private Dictionary<string, string> name = new();
		private Dictionary<string, string> description = new();
		private Dictionary<string, string> loreDescription = new();

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
				SelectedLanguageCodes.Add(key);
				name.TryAdd(key, string.Empty);
				description.TryAdd(key, string.Empty);
				loreDescription.TryAdd(key, string.Empty);
			}
			StateHasChanged();
		}
	}
}
