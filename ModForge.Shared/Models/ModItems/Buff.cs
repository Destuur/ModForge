using ModForge.Shared.Models.Attributes;
using ModForge.Shared.Models.Localizations;

namespace ModForge.Shared.Models.ModItems
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

		public Buff(string id, IList<string> perkIds, string path, IList<IAttribute> attributes, Localization localization)
		{
			Id = id;
			LinkedIds = perkIds;
			Path = path;
			Attributes = attributes;
			Localization = localization;
		}

		public string Id { get; set; } = string.Empty;
		public IList<string> LinkedIds { get; set; } = new List<string>();
		public string Path { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public IList<IAttribute> Attributes { get; set; } = new List<IAttribute>();
		public Localization Localization { get; set; } = new();

		public IModItem GetDeepCopy(IModItem modItem)
		{
			return new Buff(modItem.Id, modItem.LinkedIds, modItem.Path, modItem.Attributes.Select(attr => attr.DeepClone()).ToList(), modItem.Localization.DeepClone());
		}
	}
}
