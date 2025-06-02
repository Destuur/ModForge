using KCD2.ModForge.Shared.Models.Attributes;
using KCD2.ModForge.Shared.Models.Localizations;

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

		public Buff(string id, string path)
		{
			Id = id;
			Path = path;
		}

		public Buff(string path, IEnumerable<IAttribute> attributes)
		{
			Id = attributes.FirstOrDefault(attr => attr.Name == "buff_id")?.Value.ToString() ?? string.Empty;
			Path = path;
			Attributes = attributes.ToList();
		}

		public Buff(string id, string path, IList<IModItem> modItems, IList<IAttribute> attributes, Localization localization)
		{
			Id = id;
			Path = path;
			Attributes = attributes;
			ModItems = modItems;
			Localization = localization;
		}

		public string Id { get; set; } = string.Empty;
		public string Path { get; set; } = string.Empty;
		public IList<IAttribute> Attributes { get; set; } = new List<IAttribute>();
		public IList<IModItem> ModItems { get; set; } = new List<IModItem>();
		public Localization Localization { get; set; } = new();

		public static Buff GetDeepCopy(Buff buff)
		{
			return new Buff(buff.Id, buff.Path, buff.ModItems.ToList(), buff.Attributes.Select(attr => attr.DeepClone()).ToList(), buff.Localization.DeepClone());
		}
	}
}
