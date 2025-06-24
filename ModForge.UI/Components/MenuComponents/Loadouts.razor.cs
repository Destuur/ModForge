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

			foreach (var mod in ModService.ModCollection)
			{
				mods.Add(new DropItem() { Mod = mod, Selector = "1" });
			}

			foreach (var mod in ModService.ExternalModCollection)
			{
				mods.Add(new DropItem() { Mod = mod, Selector = "1" });
			}

			selectedSavefile = "Savefile 1";

			foreach (var savefile in savefiles)
			{
				loadouts.Add(savefile, new List<DropItem>());
			}

			try
			{

				foreach (var key in loadouts.Keys)
				{
					loadouts[key] = UserConfigurationService.Current.Loadouts[key].ToList();
				}
			}
			catch (Exception e)
			{
				Logger.LogError($"Could not add loadouts to current collection of loadouts.");
			}

			buffParams = GetBuffParams();
		}

	}
}