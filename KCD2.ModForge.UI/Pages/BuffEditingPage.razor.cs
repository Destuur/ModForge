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
	public partial class BuffEditingPage
	{
		private Buff editingBuff;
		private Buff originalBuff;

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

		private async Task SaveBuff(MouseEventArgs args)
		{
			var modBuff = new Buff(originalBuff.Id, originalBuff.Path);
			if (editingBuff is null || originalBuff is null)
			{
				return;
			}

			if (ModService is null)
			{
				return;
			}

			modBuff.Attributes = GetChangedAttributes();
			modBuff.Localization = GetChangedLocalizations();
			ModService.AddModItem(modBuff);
		}

		private Localization GetChangedLocalizations()
		{
			var modLocalization = new Localization();
			modLocalization.LoreDescriptions = FilterLocalizations(originalBuff.Localization.LoreDescriptions, editingBuff.Localization.LoreDescriptions);
			modLocalization.Names = FilterLocalizations(originalBuff.Localization.Names, editingBuff.Localization.Names);
			modLocalization.Descriptions = FilterLocalizations(originalBuff.Localization.Descriptions, editingBuff.Localization.Descriptions);
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
			foreach (var originalAttribute in originalBuff.Attributes)
			{
				var editingAttribute = editingBuff.Attributes.FirstOrDefault(x => x.Name == originalAttribute.Name);

				if (editingAttribute is null)
				{
					continue;
				}

				if (editingBuff is null)
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

			originalBuff = XmlToJsonService.Buffs!.FirstOrDefault(x => x.Id == Id)!;
			editingBuff = Buff.GetDeepCopy(originalBuff);
		}
	}
}
