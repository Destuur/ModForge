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

		private void SaveItem()
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

			//modBuff.Attributes = GetEssentialAttributes();
			modBuff.Attributes = editingBuff.Attributes;
			modBuff.Localization = GetChangedLocalizations();
			modBuff.Name = originalBuff.Localization.GetName("en");
			ModService.AddModItem(modBuff);
		}

		private async Task SaveBuff()
		{
			SaveItem();
			await NavigationService.NavigateToAsync($"/moditems/{ModService.GetMod().ModId}");
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
				await NavigationService.NavigateToAsync("/modoverview");
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
				await NavigationService.NavigateToAsync($"/moditems/{ModService.GetMod().ModId}");
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
