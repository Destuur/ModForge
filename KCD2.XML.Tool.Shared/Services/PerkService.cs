using KCD2.XML.Tool.Shared.Models;
using KCD2.XML.Tool.Shared.Mods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCD2.XML.Tool.Shared.Services
{
	public class PerkService
	{
		private List<Perk> perks = new();

		public void AddPerk(Perk perk)
		{
			perks.Add(perk);
		}

		public Perk GetPerk(string id)
		{
			var perk = perks.FirstOrDefault(x => x.Id == id);
			return perk;
		}

		public IEnumerable<Perk> GetAllPerks()
		{
			return perks.ToList();
		}
	}
}
