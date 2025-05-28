using KCD2.ModForge.Shared.Models.Attributes;
using KCD2.ModForge.Shared.Mods;
using System.Xml.Linq;

namespace KCD2.ModForge.Shared.Models.ModItems
{
	public class Perk : IModItem
	{
		public Perk(string path)
		{
			Id = Guid.NewGuid().ToString();
			Path = path;
		}

		public Perk(string path, IEnumerable<IAttribute> attributes)
		{
			Id = attributes.FirstOrDefault(attr => attr.Name == "perk_id")?.Value.ToString() ?? string.Empty;
			Path = path;
			Attributes = attributes.ToList();
		}

		public string Id { get; private set; } = string.Empty;
		public string Path { get; private set; } = string.Empty;
		public IList<IModItem> ModItems { get; } = new List<IModItem>();
		public IList<IAttribute> Attributes { get; } = new List<IAttribute>();
		public Localization Localization { get; } = new();
	}
}
