using KCD2.ModForge.Shared.Models.Attributes;
using KCD2.ModForge.Shared.Mods;

namespace KCD2.ModForge.Shared.Models.ModItems
{
	public class Buff : IModItem
	{
		public Buff()
		{

		}

		public Buff(string path)
		{
			Id = Guid.NewGuid().ToString();
			Path = path;
		}

		public Buff(string path, IEnumerable<IAttribute> attributes)
		{
			Id = attributes.FirstOrDefault(attr => attr.Name == "buff_id")?.Value.ToString() ?? string.Empty;
			Path = path;
			Attributes = attributes.ToList();
		}

		public string Id { get; set; } = string.Empty;
		public string Path { get; set; } = string.Empty;
		public IList<IAttribute> Attributes { get; set; } = new List<IAttribute>();
		public IList<IModItem> ModItems { get; set; } = new List<IModItem>();
		public Localization Localization { get; set; } = new();
	}
}
