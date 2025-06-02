using System.Collections.ObjectModel;

namespace KCD2.ModForge.Shared.Models.Mods
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

		internal ModDescription? GetMod(string modId)
		{
			return Items.FirstOrDefault(x => x.ModId == modId);
		}
	}
}
