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
		private string name;
		private string description;
		private string loreDescription;

		[Parameter]
		public IModItem ModItem { get; set; }
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
			}
			StateHasChanged();
		}
	}
}
