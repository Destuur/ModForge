using ModForge.Shared.Models.Abstractions;
using ModForge.Shared.Models.Localizations;
using Newtonsoft.Json;

namespace ModForge.Shared.Models.ModItems
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

		public Perk(string path, List<IAttribute> attributes)
		{
			Id = attributes.FirstOrDefault(attr => attr.Name == "perk_id")?.Value.ToString() ?? string.Empty;
			Path = path;
			Attributes = attributes.ToList();
		}

		public Perk(string id, string idKey, string path, List<string> linkedIds, List<IAttribute> attributes, Localization localization)
		{
			Id = id;
			IdKey = idKey;
			Path = path;
			LinkedIds = linkedIds;
			Attributes = attributes;
			Localization = localization;
		}

		public string Id { get; set; } = string.Empty;
		public string IdKey { get; set; }
		public string Path { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public List<string> LinkedIds { get; set; } = new();
		public List<IAttribute> Attributes { get; set; } = new();
		public Localization Localization { get; set; } = new();

		public IModItem GetDeepCopy()
		{
			return new Perk(Id, IdKey, Path, LinkedIds, Attributes.Select(attr => attr.DeepClone()).ToList(), Localization.DeepClone());
		}
	}
}
