using System.Collections.ObjectModel;

namespace ModForge.Shared.Models.Mods
{
	public class ModCollection : Collection<ModDescription>
	{
		public void AddMod(ModDescription mod)
		{
			if (mod is null)
			{
				return;
			}

			if (Items.Contains(mod))
			{
				return;
			}

			Items.Add(mod);
		}

		public void RemoveMod(ModDescription mod)
		{
			if (mod is null)
				return;

			var itemToRemove = Items.FirstOrDefault(item => item.Id == mod.Id);
			if (itemToRemove != null)
			{
				Items.Remove(itemToRemove);
			}
		}

		internal ModDescription? GetMod(string modId)
		{
			return Items.FirstOrDefault(x => x.Id == modId);
		}
	}
}
