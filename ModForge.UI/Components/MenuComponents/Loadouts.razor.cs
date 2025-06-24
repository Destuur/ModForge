using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using ModForge.Shared.Models.Attributes;
using ModForge.Shared.Models.ModItems;
using ModForge.Shared.Models.Mods;
using ModForge.Shared.Services;
using MudBlazor;

namespace ModForge.UI.Components.MenuComponents
{
	public partial class Loadouts
	{
		private List<DropItem> mods = new();
		private List<BuffParam> buffParams;
		private Dictionary<string, List<DropItem>> loadouts = new();
		private string[] savefiles = { "Savefile 1", "Savefile 2", "Savefile 3", "Savefile 4", "Savefile 5" };
		private MudDropContainer<DropItem> container;
		private string selectedSavefile;

		[Parameter]
		public EventCallback<Type> ChangeChildContent { get; set; }
		[Inject]
		public UserConfigurationService UserConfigurationService { get; set; }
		[Inject]
		public ModService ModService { get; set; }
		[Inject]
		public ILogger<Loadouts> Logger { get; set; }
		[Inject]
		public ISnackbar Snackbar { get; set; }
		public string SelectedSavefile
		{
			get => selectedSavefile;
			set
			{
				if (value is null)
				{
					return;
				}
				selectedSavefile = value;
				GetLoadout(value);
			}
		}

		private void SaveLoadouts()
		{
			UserConfigurationService.Current.Loadouts = loadouts;
			UserConfigurationService.Save();

			Snackbar.Add("Loadouts successfully saved", Severity.Success);
		}

		private void SetLoadout()
		{
			if (loadouts[selectedSavefile].Count == 0)
			{
				Snackbar.Add($"Stop yanking my pizzle — there are no mods to prep for your journey.", Severity.Warning);
			}
			else
			{
				try
				{
					UserConfigurationService.WriteLoadout(loadouts[selectedSavefile]);
					Snackbar.Add($"You're all set! Loadout from '{selectedSavefile}' is ready.", Severity.Success);
				}
				catch (Exception e)
				{
					Logger.LogError(@"Loadout could not be written to mod_order.txt.");
					Snackbar.Add($"Oops. Something went wrong, mate.", Severity.Success);
				}
			}
		}

		private void ItemUpdated(MudItemDropInfo<DropItem> dropItem)
		{
			dropItem.Item.Selector = dropItem.DropzoneIdentifier;

			if (dropItem.Item.Selector == "1")
			{
				loadouts[selectedSavefile].Remove(dropItem.Item);
				buffParams = GetBuffParams();
			}
			else
			{
				loadouts[selectedSavefile].Add(dropItem.Item);
				buffParams = GetBuffParams();
			}
		}

		private void ClearLoadout()
		{
			// Kopiere die Liste, um Modifikationen während der Iteration zu vermeiden
			var itemsToRemove = loadouts[selectedSavefile].ToList();

			foreach (var dropItem in itemsToRemove)
			{
				// Setze Selector auf "1" (Available Mods)
				dropItem.Selector = "1";
			}

			// Liste leeren, Referenz bleibt erhalten
			loadouts[selectedSavefile].Clear();

			// UI ggf. aktualisieren
			container?.Refresh();
			buffParams = GetBuffParams();
		}

		private void GetLoadout(string savefile)
		{
			foreach (var dropItem in mods)
			{
				dropItem.Selector = "1";
			}

			if (loadouts[selectedSavefile] is null)
			{
				return;
			}

			foreach (var dropItem in loadouts[selectedSavefile])
			{
				var mod = mods.FirstOrDefault(x => x.Mod.Id == dropItem.Mod.Id);

				if (mod is null)
				{
					loadouts[selectedSavefile].Remove(dropItem);
					continue;
				}
				else
				{
					mod.Selector = "2";
				}
			}
			container.Refresh();
			buffParams = GetBuffParams();
		}

		private List<BuffParam> GetBuffParams()
		{
			var foundList = new List<BuffParam>();

			foreach (var dropItem in loadouts[selectedSavefile])
			{
				foreach (var modItem in dropItem.Mod.ModItems)
				{
					foreach (var attribute in modItem.Attributes)
					{
						if (attribute is Attribute<IList<BuffParam>> foundBuffParams)
						{
							foreach (var buffParam in foundBuffParams.Value)
							{
								if (buffParam is null)
								{
									continue;
								}

								var existingBuffParam = foundList.FirstOrDefault(x => x.Key == buffParam.Key);

								if (existingBuffParam is null)
								{
									foundList.Add(buffParam.DeepClone());
								}
								else
								{
									existingBuffParam.Value += buffParam.Value;
								}
							}
						}
					}
				}
			}
			return foundList;
		}

		protected override void OnInitialized()
		{
			base.OnInitialized();
			PrepareModCollection();
			PrepareLoadouts();
			buffParams = GetBuffParams();
		}

		private void PrepareModCollection()
		{
			foreach (var mod in ModService.ModCollection)
			{
				mods.Add(new DropItem() { Mod = mod, Selector = "1" });
			}

			foreach (var mod in ModService.ExternalModCollection)
			{
				mods.Add(new DropItem() { Mod = mod, Selector = "1" });
			}
		}

		private void PrepareLoadouts()
		{
			selectedSavefile = "Savefile 1";

			foreach (var savefile in savefiles)
			{
				loadouts.Add(savefile, new List<DropItem>());
			}

			try
			{
				foreach (var key in loadouts.Keys)
				{
					// Lade die gespeicherten Loadouts
					var savedItems = UserConfigurationService.Current.Loadouts[key];
					loadouts[key] = new List<DropItem>();

					foreach (var savedItem in savedItems)
					{
						// Finde das passende DropItem in mods anhand der Mod-Id
						var modItem = mods.FirstOrDefault(x => x.Mod.Id == savedItem.Mod.Id);
						if (modItem != null)
						{
							modItem.Selector = "2";
							loadouts[key].Add(modItem);
						}
					}
				}
			}
			catch (Exception e)
			{
				Logger.LogError($"Could not add loadouts to current collection of loadouts.");
			}
		}
	}
}