using ModForge.Shared.Models.Abstractions;
using ModForge.Shared.Models.Localizations;

namespace ModForge.Shared.Models.ModItems
{
	public abstract class WeaponClass : IModItem
	{
		public WeaponClass()
		{

		}

		public WeaponClass(string id, string path, List<string> linkedIds, List<IAttribute> attributes, Localization localization)
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

		public abstract IModItem GetDeepCopy();
	}

	public class MeleeWeaponClass : WeaponClass
	{
		public MeleeWeaponClass()
		{
		}

		public MeleeWeaponClass(string id, string path, List<string> linkedIds, List<IAttribute> attributes, Localization localization) : base(id, path, linkedIds, attributes, localization)
		{
		}

		public override IModItem GetDeepCopy()
		{
			return new MeleeWeaponClass(Id, Path, LinkedIds, Attributes.Select(attr => attr.DeepClone()).ToList(), Localization.DeepClone());
		}
	}

	public class MissileWeaponClass : WeaponClass
	{
		public MissileWeaponClass()
		{
		}

		public MissileWeaponClass(string id, string path, List<string> linkedIds, List<IAttribute> attributes, Localization localization) : base(id, path, linkedIds, attributes, localization)
		{
		}

		public override IModItem GetDeepCopy()
		{
			return new MissileWeaponClass(Id, Path, LinkedIds, Attributes.Select(attr => attr.DeepClone()).ToList(), Localization.DeepClone());
		}
	}
}
