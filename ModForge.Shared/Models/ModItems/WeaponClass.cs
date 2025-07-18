using ModForge.Shared.Models.Abstractions;
using ModForge.Shared.Models.Localizations;

namespace ModForge.Shared.Models.ModItems
{
	public abstract class WeaponClass : IModItem
	{
		public WeaponClass()
		{

		}

		public WeaponClass(string id, string idKey, string path, List<string> linkedIds, List<IAttribute> attributes, Localization localization)
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
		public List<string> LinkedIds { get; set; } = new();
		public List<IAttribute> Attributes { get; set; } = new();
		public Localization Localization { get; set; } = new();

		public abstract IModItem GetDeepCopy();
	}
}
