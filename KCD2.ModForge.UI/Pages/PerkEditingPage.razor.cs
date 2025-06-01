using KCD2.ModForge.Shared.Models.Attributes;
using KCD2.ModForge.Shared.Models.Localizations;
using KCD2.ModForge.Shared.Models.ModItems;
using KCD2.ModForge.Shared.Services;
using KCD2.ModForge.UI.Components.DialogComponents;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace KCD2.ModForge.UI.Pages
{
	public partial class PerkEditingPage
	{
		private Perk editingPerk;
		private Perk originalPerk;

		[Parameter]
		public string Id { get; set; }
		[Inject]
		public XmlToJsonService XmlToJsonService { get; set; }
		[Inject]
		public NavigationService NavigationService { get; set; }
		[Inject]
		public ModService ModService { get; set; }
		[Inject]
		public IDialogService DialogService { get; set; }

		private async Task SavePerk(MouseEventArgs args)
		{
			var modPerk = new Perk(originalPerk.Id, originalPerk.Path);
			if (editingPerk is null || originalPerk is null)
			{
				return;
			}

			if (ModService is null)
			{
				return;
			}

			modPerk.Attributes = GetChangedAttributes();
			modPerk.Localization = GetChangedLocalizations();
			ModService.AddModItem(modPerk);
		}

		private Localization GetChangedLocalizations()
		{
			var modLocalization = new Localization();
			modLocalization.LoreDescriptions = FilterLocalizations(originalPerk.Localization.LoreDescriptions, editingPerk.Localization.LoreDescriptions);
			modLocalization.Names = FilterLocalizations(originalPerk.Localization.Names, editingPerk.Localization.Names);
			modLocalization.Descriptions = FilterLocalizations(originalPerk.Localization.Descriptions, editingPerk.Localization.Descriptions);
			return modLocalization;
		}

		private Dictionary<string, string> FilterLocalizations(Dictionary<string, string>? originalLocalizations, Dictionary<string, string>? modifiedLocalizations)
		{
			var modDictionary = new Dictionary<string, string>();

			foreach (var originalLocalization in originalLocalizations)
			{

				var key = originalLocalization.Key;

				if (modifiedLocalizations.TryGetValue(key, out string modifiedLocalizationValue) == false)
				{
					continue;
				}

				if (string.IsNullOrEmpty(modifiedLocalizationValue))
				{
					modifiedLocalizations.Remove(key);
					continue;
				}

				if (modifiedLocalizationValue.Trim().Equals(originalLocalization.Value) == false)
				{

					modDictionary.TryAdd(key, modifiedLocalizationValue);
				}
			}

			return modDictionary;
		}

		private IList<IAttribute> GetChangedAttributes()
		{
			var modList = new List<IAttribute>();
			foreach (var originalAttribute in originalPerk.Attributes)
			{
				var editingAttribute = editingPerk.Attributes.FirstOrDefault(x => x.Name == originalAttribute.Name);

				if (editingAttribute is null)
				{
					continue;
				}

				if (editingPerk is null)
				{
					continue;
				}

				if (editingAttribute.Value.Equals(originalAttribute.Value) == false)
				{
					modList.Add(editingAttribute);
				}
			}

			return modList;
		}

		private async Task Cancel()
		{
			var parameters = new DialogParameters<ChangesDetectedDialog>
		{
			{ x => x.ContentText, "Do you really want to discard all changes?" },
			{ x => x.ButtonText, "Discard" }
		};

			var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

			var dialog = await DialogService.ShowAsync<ChangesDetectedDialog>("Discard Changes", parameters, options);
			var result = await dialog.Result;

			if (result.Canceled == false)
			{
				await NavigationService.GoBackAsync();
			}
		}

		protected override void OnInitialized()
		{
			base.OnInitialized();
			if (XmlToJsonService is null)
			{
				return;
			}

			originalPerk = XmlToJsonService.Perks!.FirstOrDefault(x => x.Id == Id)!;
			editingPerk = Perk.GetDeepCopy(originalPerk);
		}
	}
}
