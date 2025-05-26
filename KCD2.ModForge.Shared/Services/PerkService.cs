using KCD2.ModForge.Shared.Models.ModItems;

namespace KCD2.ModForge.Shared.Services
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
