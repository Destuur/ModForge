using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using ModForge.Shared.Models.Attributes;
using ModForge.Shared.Models.Localizations;
using ModForge.Shared.Models.ModItems;
using ModForge.Shared.Services;
using ModForge.UI.Components.DialogComponents;
using MudBlazor;
using System.Text.Json;

namespace ModForge.UI.Pages
{
	public partial class PerkEditingPage
	{
		private Perk editingPerk;
		private Perk originalPerk;
		private bool canCheckout;
		private int selectedTab;

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

		private void SaveItem()
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

			//modPerk.Attributes = GetEssentialAttributes();
			modPerk.Attributes = editingPerk.Attributes;
			modPerk.Localization = GetChangedLocalizations();
			modPerk.Name = originalPerk.Localization.GetName("en");
			ModService.AddModItem(modPerk);
		}

		private async Task SavePerk()
		{
			SaveItem();
			await NavigationService.NavigateToAsync($"/moditems/{ModService.GetCurrentMod().ModId}");
		}

		private async Task Checkout()
		{
			SaveItem();

			var parameters = new DialogParameters<MoreModItemsDialog>
			{
				{ x => x.ContentText, "Though have yanked enough pizzles? Leave then, and create your mod!" },
				{ x => x.ButtonText, "Create Mod" }
			};

			var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

			var dialog = await DialogService.ShowAsync<MoreModItemsDialog>("Create Your Mod", parameters, options);
			var result = await dialog.Result;

			if (result.Canceled == false)
			{
				canCheckout = true;
				await NavigationService.NavigateToAsync("/modoverview");
			}
		}

		private Localization GetChangedLocalizations()
		{
			var modLocalization = new Localization
			{
				LoreDescriptions = FilterNestedLocalizations(originalPerk.Localization.LoreDescriptions, editingPerk.Localization.LoreDescriptions),

				Names = FilterNestedLocalizations(originalPerk.Localization.Names, editingPerk.Localization.Names),

				Descriptions = FilterNestedLocalizations(originalPerk.Localization.Descriptions, editingPerk.Localization.Descriptions)
			};

			return modLocalization;
		}

		private Dictionary<string, Dictionary<string, string>> FilterNestedLocalizations(Dictionary<string, Dictionary<string, string>> original, Dictionary<string, Dictionary<string, string>> edited)
		{
			var result = new Dictionary<string, Dictionary<string, string>>();

			foreach (var lang in edited)
			{
				if (!original.TryGetValue(lang.Key, out var originalInner))
				{
					// komplette Sprache übernehmen
					result[lang.Key] = new Dictionary<string, string>(lang.Value);
					continue;
				}

				foreach (var kvp in lang.Value)
				{
					if (!originalInner.TryGetValue(kvp.Key, out var origValue) || origValue != kvp.Value)
					{
						if (!result.ContainsKey(lang.Key))
							result[lang.Key] = new Dictionary<string, string>();

						result[lang.Key][kvp.Key] = kvp.Value;
					}
				}
			}

			return result;
		}

		private async Task<bool> ConfirmNavigation(LocationChangingContext context)
		{
			var jsonA = JsonSerializer.Serialize(editingPerk);
			var jsonB = JsonSerializer.Serialize(originalPerk);

			if (jsonA == jsonB)
			{
				return true;
			}

			if (canCheckout)
			{
				canCheckout = false;
				return true;
			}

			var parameters = new DialogParameters<ChangesDetectedDialog>()
			{
				{ x => x.ContentText, "If you leave now, you might lose some changes.\r\nDo you want to continue or stay on this page?" },
				{ x => x.ButtonText, "Leave Anyway" }
			};

			var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

			var dialog = await DialogService.ShowAsync<ChangesDetectedDialog>("Leave Page?", parameters, options);
			var result = await dialog.Result;

			if (result.Canceled)
			{
				context.PreventNavigation();
			}

			return true;
		}

		private IList<IAttribute> GetEssentialAttributes()
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

				if (editingAttribute.Name == "perk_id")
				{
					modList.Add(editingAttribute);
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
				await NavigationService.NavigateToAsync($"/moditems/{ModService.GetCurrentMod().ModId}");
			}
		}

		protected override void OnInitialized()
		{
			base.OnInitialized();
			if (XmlToJsonService is null)
			{
				return;
			}


			originalPerk = XmlToJsonService.Perks!.FirstOrDefault(x => x.Id == Id)! as Perk;

			if (ModService.Mod.ModItems.FirstOrDefault(x => x.Id == Id) is not null)
			{
				originalPerk = ModService.Mod.ModItems.FirstOrDefault(x => x.Id == Id) as Perk;
			}

			editingPerk = Perk.GetDeepCopy(originalPerk);
			StateHasChanged();
		}
	}
}
