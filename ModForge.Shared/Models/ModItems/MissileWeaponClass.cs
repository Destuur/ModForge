using ModForge.Shared.Models.Abstractions;
using ModForge.Shared.Models.Localizations;

namespace ModForge.Shared.Models.ModItems
{
	public class MissileWeaponClass : WeaponClass
	{
		public MissileWeaponClass()
		{
		}

		public MissileWeaponClass(string id, string idKey, string path, List<string> linkedIds, List<IAttribute> attributes, Localization localization) : base(id, idKey, path, linkedIds, attributes, localization)
		{
		}

		public override IModItem GetDeepCopy()
		{
			return new MissileWeaponClass(Id, Path, IdKey, LinkedIds, Attributes.Select(attr => attr.DeepClone()).ToList(), Localization.DeepClone());
		}
	}
}
