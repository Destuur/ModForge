using ModForge.Shared.Models.Attributes;
using ModForge.Shared.Models.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModForge.Shared.Models.ModItems
{
	public class MeleeWeapon : IModItem
	{
		public string Id { get; set; }
		public string Path { get; set; }
		public IList<string> LinkedIds { get; set; }
		public IList<IAttribute> Attributes { get; set; }
		public Localization Localization { get; set; }
	}

	public class NPCTool : IModItem
	{
		public string Id { get; set; }
		public string Path { get; set; }
		public IList<string> LinkedIds { get; set; }
		public IList<IAttribute> Attributes { get; set; }
		public Localization Localization { get; set; }
	}

	public class MiscItem : IModItem
	{
		public string Id { get; set; }
		public string Path { get; set; }
		public IList<string> LinkedIds { get; set; }
		public IList<IAttribute> Attributes { get; set; }
		public Localization Localization { get; set; }
	}

	public class Hood : IModItem
	{
		public string Id { get; set; }
		public string Path { get; set; }
		public IList<string> LinkedIds { get; set; }
		public IList<IAttribute> Attributes { get; set; }
		public Localization Localization { get; set; }
	}

	public class Armor : IModItem
	{
		public string Id { get; set; }
		public string Path { get; set; }
		public IList<string> LinkedIds { get; set; }
		public IList<IAttribute> Attributes { get; set; }
		public Localization Localization { get; set; }
	}

	public class MissileWeapon : IModItem
	{
		public string Id { get; set; }
		public string Path { get; set; }
		public IList<string> LinkedIds { get; set; }
		public IList<IAttribute> Attributes { get; set; }
		public Localization Localization { get; set; }
	}

	public class Document : IModItem
	{
		public string Id { get; set; }
		public string Path { get; set; }
		public IList<string> LinkedIds { get; set; }
		public IList<IAttribute> Attributes { get; set; }
		public Localization Localization { get; set; }
	}

	public class Herb : IModItem
	{
		public string Id { get; set; }
		public string Path { get; set; }
		public IList<string> LinkedIds { get; set; }
		public IList<IAttribute> Attributes { get; set; }
		public Localization Localization { get; set; }
	}

	public class Food : IModItem
	{
		public string Id { get; set; }
		public string Path { get; set; }
		public IList<string> LinkedIds { get; set; }
		public IList<IAttribute> Attributes { get; set; }
		public Localization Localization { get; set; }
	}

	public class Helmet : IModItem
	{
		public string Id { get; set; }
		public string Path { get; set; }
		public IList<string> LinkedIds { get; set; }
		public IList<IAttribute> Attributes { get; set; }
		public Localization Localization { get; set; }
	}

	public class Die : IModItem
	{
		public string Id { get; set; }
		public string Path { get; set; }
		public IList<string> LinkedIds { get; set; }
		public IList<IAttribute> Attributes { get; set; }
		public Localization Localization { get; set; }
	}

	public class Ammo : IModItem
	{
		public string Id { get; set; }
		public string Path { get; set; }
		public IList<string> LinkedIds { get; set; }
		public IList<IAttribute> Attributes { get; set; }
		public Localization Localization { get; set; }
	}

	public class ItemAlias : IModItem
	{
		public string Id { get; set; }
		public string Path { get; set; }
		public IList<string> LinkedIds { get; set; }
		public IList<IAttribute> Attributes { get; set; }
		public Localization Localization { get; set; }
	}

	public class QuickSlotContainer : IModItem
	{
		public string Id { get; set; }
		public string Path { get; set; }
		public IList<string> LinkedIds { get; set; }
		public IList<IAttribute> Attributes { get; set; }
		public Localization Localization { get; set; }
	}

	public class DiceBadge : IModItem
	{
		public string Id { get; set; }
		public string Path { get; set; }
		public IList<string> LinkedIds { get; set; }
		public IList<IAttribute> Attributes { get; set; }
		public Localization Localization { get; set; }
	}

	public class CraftingMaterial : IModItem
	{
		public string Id { get; set; }
		public string Path { get; set; }
		public IList<string> LinkedIds { get; set; }
		public IList<IAttribute> Attributes { get; set; }
		public Localization Localization { get; set; }
	}

	public class Poison : IModItem
	{
		public string Id { get; set; }
		public string Path { get; set; }
		public IList<string> LinkedIds { get; set; }
		public IList<IAttribute> Attributes { get; set; }
		public Localization Localization { get; set; }
	}

	public class PickableItem : IModItem
	{
		public string Id { get; set; }
		public string Path { get; set; }
		public IList<string> LinkedIds { get; set; }
		public IList<IAttribute> Attributes { get; set; }
		public Localization Localization { get; set; }
	}

	public class Key : IModItem
	{
		public string Id { get; set; }
		public string Path { get; set; }
		public IList<string> LinkedIds { get; set; }
		public IList<IAttribute> Attributes { get; set; }
		public Localization Localization { get; set; }
	}

	public class Money : IModItem
	{
		public string Id { get; set; }
		public string Path { get; set; }
		public IList<string> LinkedIds { get; set; }
		public IList<IAttribute> Attributes { get; set; }
		public Localization Localization { get; set; }
	}

	public class KeyRing : IModItem
	{
		public string Id { get; set; }
		public string Path { get; set; }
		public IList<string> LinkedIds { get; set; }
		public IList<IAttribute> Attributes { get; set; }
		public Localization Localization { get; set; }
	}
}
