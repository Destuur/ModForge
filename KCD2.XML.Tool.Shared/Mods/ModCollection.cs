using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCD2.XML.Tool.Shared.Mods
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
