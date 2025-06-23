using Microsoft.AspNetCore.Components;
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
		private string selectedSavefile;
		private MudDropContainer<DropItem> container;

		[Parameter]
		public EventCallback<Type> ChangeChildContent { get; set; }
		[Inject]
		public ModService ModService { get; set; }

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
			selectedSavefile = savefile;

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

			buffParams = GetBuffParams();
		}

		public class DropItem
		{
			public ModDescription Mod { get; init; }
			public string Selector { get; set; }
		}
	}
}