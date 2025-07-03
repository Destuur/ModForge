namespace ModForge.Shared.Models.Enums
{

	public enum ItemCategory
	{
		Misc = 0,
		MeleeWeapon = 1,
		MissileWeapon = 2,
		Ammo = 3,
		Armor = 4,
		Food = 5,
		Money = 6,
		DiceBadge = 7,
		Document = 8,
		CraftingMaterial = 9,
		Herb = 10,
		AlchemyBase = 11,
		NpcTool = 12,
		OintmentItem = 13,
		Poison = 14,
		Die = 15,
		Helmet = 16,
		Key = 17,
		Keyring = 18,
		QuickSlotContainer = 19,
		Item = 21,
		PickableItem = 22,
		DivisibleItem = 23,
		WeaponEquip = 24,
		PlayerItem = 25,
		EquippableItem = 26,
		Weapon = 27,
		ConsumableItem = 28,
		Hood = 29
	}

	public static class ItemCategoryHelper
	{
		private static readonly Dictionary<ItemCategory, bool> _instanceableMap = new()
		{
			{ ItemCategory.Misc, true },
			{ ItemCategory.MeleeWeapon, true },
			{ ItemCategory.MissileWeapon, true },
			{ ItemCategory.Ammo, true },
			{ ItemCategory.Armor, true },
			{ ItemCategory.Food, true },
			{ ItemCategory.Money, true },
			{ ItemCategory.DiceBadge, true },
			{ ItemCategory.Document, true },
			{ ItemCategory.CraftingMaterial, true },
			{ ItemCategory.Herb, true },
			{ ItemCategory.AlchemyBase, true },
			{ ItemCategory.NpcTool, true },
			{ ItemCategory.OintmentItem, true },
			{ ItemCategory.Poison, true },
			{ ItemCategory.Die, true },
			{ ItemCategory.Helmet, true },
			{ ItemCategory.Key, true },
			{ ItemCategory.Keyring, true },
			{ ItemCategory.QuickSlotContainer, true },
			{ ItemCategory.Item, false },
			{ ItemCategory.PickableItem, false },
			{ ItemCategory.DivisibleItem, false },
			{ ItemCategory.WeaponEquip, false },
			{ ItemCategory.PlayerItem, false },
			{ ItemCategory.EquippableItem, false },
			{ ItemCategory.Weapon, false },
			{ ItemCategory.ConsumableItem, false },
			{ ItemCategory.Hood, true }
		};

		public static bool IsInstanceable(ItemCategory category)
		{
			return _instanceableMap.TryGetValue(category, out var value) && value;
		}
	}
}
