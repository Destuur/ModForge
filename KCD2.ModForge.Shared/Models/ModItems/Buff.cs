using KCD2.ModForge.Shared.Models.Attributes;
using KCD2.ModForge.Shared.Mods;

namespace KCD2.ModForge.Shared.Models.ModItems
{
	public class Buff : IModItem
	{
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
		public string Id { get; } = string.Empty;
		public string Path { get; private set; } = string.Empty;
		public IList<IAttribute> Attributes { get; }
		public IList<IModItem> ModItems { get; }
		public Localization Localization { get; } = new();
	}
}
