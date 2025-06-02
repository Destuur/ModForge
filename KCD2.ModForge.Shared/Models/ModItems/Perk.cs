using KCD2.ModForge.Shared.Models.Attributes;
using KCD2.ModForge.Shared.Models.Localizations;
using Newtonsoft.Json;

namespace KCD2.ModForge.Shared.Models.ModItems
{
	public class Perk : IModItem
	{
		public Perk()
		{

		}

		public Perk(string path)
		{
			Id = Guid.NewGuid().ToString();
			Path = path;
		}

		public Perk(string id, string path)
		{
			Id = id;
			Path = path;
		}

		public Perk(string path, IEnumerable<IAttribute> attributes)
		{
			Id = attributes.FirstOrDefault(attr => attr.Name == "perk_id")?.Value.ToString() ?? string.Empty;
			Path = path;
			Attributes = attributes.ToList();
		}

		public Perk(string id, string path, IEnumerable<IModItem> modItems, IEnumerable<IAttribute> attributes, Localization localization)
		{
			Id = id;
			Path = path;
			ModItems = modItems.ToList();
			Attributes = attributes.ToList();
			Localization = localization;
		}

		public string Id { get; set; } = string.Empty;
		public string Path { get; set; } = string.Empty;
		public string Name { get; set; }
		public IList<IModItem> ModItems { get; set; } = new List<IModItem>();
		public IList<IAttribute> Attributes { get; set; } = new List<IAttribute>();
		public Localization Localization { get; set; } = new();

		public static Perk GetDeepCopy(Perk perk)
		{
			return new Perk(perk.Id, perk.Path, perk.ModItems.ToList(), perk.Attributes.Select(attr => attr.DeepClone()).ToList(), perk.Localization.DeepClone());
		}
	}
}
