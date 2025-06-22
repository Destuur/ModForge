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
		private List<BuffParam> buffParams = new();

		[Parameter]
		public EventCallback<Type> ChangeChildContent { get; set; }
		[Inject]
		public ModService ModService { get; set; }

		private void ItemUpdated(MudItemDropInfo<DropItem> dropItem)
		{
			dropItem.Item.Selector = dropItem.DropzoneIdentifier;

			if (dropItem.Item.Selector == "1")
			{
				foreach (var modItem in dropItem.Item.Mod.ModItems)
				{
					RemoveBuffParams(modItem);
				}
			}
			else
			{
				foreach (var modItem in dropItem.Item.Mod.ModItems)
				{
					AddBuffParams(modItem);
				}
			}


		}

		private void RemoveBuffParams(IModItem modItem)
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

						var existingBuffParam = buffParams.FirstOrDefault(x => x.Key == buffParam.Key);

						if (existingBuffParam is null)
						{
							continue;
						}

						if (existingBuffParam.Value - buffParam.Value == 0)
						{
							buffParams.Remove(buffParam);
						}
						else
						{
							existingBuffParam.Value -= buffParam.Value;
						}
					}
				}
			}
		}

		private void AddBuffParams(IModItem modItem)
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

						var existingBuffParam = buffParams.FirstOrDefault(x => x.Key == buffParam.Key);

						if (existingBuffParam is null)
						{
							buffParams.Add(buffParam);
						}
						else
						{
							existingBuffParam.Value += buffParam.Value;
						}
					}
				}
			}
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
		}

		public class DropItem
		{
			public ModDescription Mod { get; init; }
			public string Selector { get; set; }
		}
	}
}