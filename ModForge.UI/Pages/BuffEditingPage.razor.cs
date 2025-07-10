using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Logging;
using ModForge.Shared.Models.Abstractions;
using ModForge.Shared.Models.Attributes;
using ModForge.Shared.Models.Localizations;
using ModForge.Shared.Models.ModItems;
using ModForge.Shared.Services;
using ModForge.UI.Components.DialogComponents;
using MudBlazor;
using System.Text.Json;

namespace ModForge.UI.Pages
{
	public partial class BuffEditingPage
	{
		private Buff editingBuff;
		private Buff originalBuff;
		private bool canCheckout;
		private int selectedTab;

		[Parameter]
		public string Id { get; set; }
		[Inject]
		public XmlService XmlToJsonService { get; set; }
		[Inject]
		public NavigationManager Navigation { get; set; }
		[Inject]
		public ModService ModService { get; set; }
		[Inject]
		public ILogger<BuffEditingPage> Logger { get; set; }
		[Inject]
		public IDialogService DialogService { get; set; }


		private void SaveItem()
		{
			if (editingBuff is null)
			{
				Logger?.LogWarning("SaveItem aborted: editingBuff is null.");
				return;
			}

			if (originalBuff is null)
			{
				Logger?.LogWarning("SaveItem aborted: originalBuff is null.");
				return;
			}

			if (ModService is null)
			{
				Logger?.LogWarning("SaveItem aborted: ModService is null.");
				return;
			}

			var modBuff = new Buff(originalBuff.Id, originalBuff.Path)
			{
				Attributes = editingBuff.Attributes,
				Localization = GetChangedLocalizations(),
				Name = originalBuff.Localization.GetName("en")
			};

			ModService.AddModItem(modBuff);
			Logger?.LogInformation($"Saved mod buff with Id: {modBuff.Id}");
		}

		private void SaveBuff()
		{
			SaveItem();

			var mod = ModService?.Mod;
			if (mod == null)
			{
				Logger?.LogWarning("Current mod is null, navigation cancelled.");
				return;
			}

			if (Navigation == null)
			{
				Logger?.LogWarning("Navigation service is null, cannot navigate after saving buff.");
				return;
			}

			Navigation.NavigateTo($"/moditems/{mod.Id}");
		}

		private async Task Checkout()
		{
			SaveItem();
			Logger?.LogInformation("Item saved before checkout.");

			var parameters = new DialogParameters<MoreModItemsDialog>
	{
		{ x => x.ContentText, "Have you pulled enough pizzles? Depart then, and shape thy mod!" },
		{ x => x.ButtonText, "Create Mod" }
	};

			var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

			var dialog = await DialogService.ShowAsync<MoreModItemsDialog>("Create Your Mod", parameters, options);
			var result = await dialog.Result;

			if (!result.Canceled)
			{
				canCheckout = true;
				Logger?.LogInformation("User confirmed checkout, navigating to mod overview.");
				Navigation.NavigateTo($"/modoverview/{ModService.Mod.Id}");
			}
			else
			{
				Logger?.LogInformation("User canceled the checkout dialog.");
			}
		}

		private Localization GetChangedLocalizations()
		{
			var modLocalization = new Localization
			{
				LoreDescriptions = FilterNestedLocalizations(originalBuff.Localization.LoreDescriptions, editingBuff.Localization.LoreDescriptions),

				Names = FilterNestedLocalizations(originalBuff.Localization.Names, editingBuff.Localization.Names),

				Descriptions = FilterNestedLocalizations(originalBuff.Localization.Descriptions, editingBuff.Localization.Descriptions)
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

		private IList<IAttribute> GetEssentialAttributes()
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

				if (editingAttribute.Name == "buff_id")
				{
					modList.Add(editingAttribute);
					continue;
				}

				if (editingAttribute is Attribute<IList<BuffParam>> editingBuffParams &&
					originalAttribute is Attribute<IList<BuffParam>> originalBuffParams)
				{
					var modBuffParams = new Attribute<IList<BuffParam>>(editingBuffParams.Name, new List<BuffParam>());

					foreach (var original in originalBuffParams.Value)
					{
						var edited = editingBuffParams.Value.FirstOrDefault(x => x.Key == original.Key);

						if (edited is null)
							continue;

						bool keyChanged = edited.Key != original.Key;
						bool opChanged = edited.Operation != original.Operation;
						bool valueChanged = edited.Value != original.Value;

						if (keyChanged || opChanged || valueChanged)
						{
							modBuffParams.Value.Add(new BuffParam(edited.Key, edited.Operation, edited.Value));
						}
					}

					if (modBuffParams.Value.Count > 0)
						modList.Add(modBuffParams);
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
			Logger?.LogInformation("Cancel operation initiated: showing discard confirmation dialog.");

			var parameters = new DialogParameters<ExitDialog>
	{
		{ x => x.ContentText, "Do you really want to discard all changes?" }
	};

			var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

			var dialog = await DialogService.ShowAsync<ExitDialog>("Discard Changes", parameters, options);
			var result = await dialog.Result;

			if (!result.Canceled)
			{
				Logger?.LogInformation("User confirmed discard. Navigating back to mod items.");
				Navigation.NavigateTo($"/moditems/{ModService.Mod.Id}");
			}
			else
			{
				Logger?.LogInformation("User canceled discard dialog. No navigation performed.");
			}
		}

		protected override void OnInitialized()
		{
			base.OnInitialized();
			Logger?.LogInformation("Component initialization started.");

			if (XmlToJsonService is null)
			{
				Logger?.LogWarning("XmlToJsonService is null. Initialization aborted.");
				return;
			}

			originalBuff = XmlToJsonService.Buffs?.FirstOrDefault(x => x.Id == Id) as Buff;
			Logger?.LogInformation(originalBuff != null
				? $"Original buff loaded from XmlToJsonService with Id {Id}."
				: $"No original buff found in XmlToJsonService for Id {Id}.");

			var modBuff = ModService.Mod.ModItems.FirstOrDefault(x => x.Id == Id) as Buff;
			if (modBuff is not null)
			{
				originalBuff = modBuff;
				Logger?.LogInformation($"Original buff overridden with mod item buff for Id {Id}.");
			}

			//editingBuff = Buff.GetDeepCopy(originalBuff);
			Logger?.LogInformation("Created deep copy of buff for editing.");

			StateHasChanged();
			Logger?.LogInformation("Component state updated.");
		}

	}
}
