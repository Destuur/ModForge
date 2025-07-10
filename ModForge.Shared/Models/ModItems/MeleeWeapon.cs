using ModForge.Shared.Models.Abstractions;
using ModForge.Shared.Models.Localizations;

namespace ModForge.Shared.Models.ModItems
{
	public class MeleeWeapon : IModItem, IItem
	{
		public MeleeWeapon()
		{

		}

		public MeleeWeapon(string id, string path, List<string> linkedIds, List<IAttribute> attributes, Localization localization)
		{
			Id = id;
			Path = path;
			LinkedIds = linkedIds;
			Attributes = attributes;
			Localization = localization;
		}

		public string Id { get; set; } = string.Empty;
		public string Path { get; set; } = string.Empty;
		public List<string> LinkedIds { get; set; } = new();
		public List<IAttribute> Attributes { get; set; } = new();
		public Localization Localization { get; set; } = new();

		public IModItem GetDeepCopy()
		{
			return new MeleeWeapon(Id, Path, LinkedIds, Attributes.Select(attr => attr.DeepClone()).ToList(), Localization.DeepClone());
		}
	}
}
