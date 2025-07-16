using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModForge.Shared.Models.Mapping
{
	public record XmlWriteInfo(string FilePrefix, string GroupName, string ElementName);


	public static class XmlStructureMapping
	{
		public static readonly Dictionary<string, XmlWriteInfo> ElementMapping = new(StringComparer.OrdinalIgnoreCase)
		{
			{ "MeleeWeapon", new XmlWriteInfo("item", "ItemClasses", "MeleeWeapon") },
			{ "Armor", new XmlWriteInfo("item", "ItemClasses", "Armor") },
			{ "Document", new XmlWriteInfo("item", "ItemClasses", "Document") },
			{ "Ammo", new XmlWriteInfo("item", "ItemClasses", "Ammo") },
			{ "CraftingMaterial", new XmlWriteInfo("item", "ItemClasses", "CraftingMaterial") },
			{ "DiceBadge", new XmlWriteInfo("item", "ItemClasses", "DiceBadge") },
			{ "Die", new XmlWriteInfo("item", "ItemClasses", "Die") },
			{ "Food", new XmlWriteInfo("item", "ItemClasses", "Food") },
			{ "Helmet", new XmlWriteInfo("item", "ItemClasses", "Helmet") },
			{ "Herb", new XmlWriteInfo("item", "ItemClasses", "Herb") },
			{ "Hood", new XmlWriteInfo("item", "ItemClasses", "Hood") },
			{ "ItemAlias", new XmlWriteInfo("item", "ItemClasses", "ItemAlias") },
			{ "Key", new XmlWriteInfo("item", "ItemClasses", "Key") },
			{ "KeyRing", new XmlWriteInfo("item", "ItemClasses", "KeyRing") },
			{ "MiscItem", new XmlWriteInfo("item", "ItemClasses", "MiscItem") },
			{ "MissileWeapon", new XmlWriteInfo("item", "WeaponClasss", "MissileWeapon") },
			{ "Money", new XmlWriteInfo("item", "ItemClasses", "Money") },
			{ "NPCTool", new XmlWriteInfo("item", "ItemClasses", "NPCTool") },
			{ "PickableItem", new XmlWriteInfo("item", "ItemClasses", "PickableItem") },
			{ "Poison", new XmlWriteInfo("item", "ItemClasses", "Poison") },
			{ "QuickSlotContainer", new XmlWriteInfo("item", "ItemClasses", "QuickSlotContainer") },
			{ "MeleeWeaponClass", new XmlWriteInfo("item", "WeaponClasss", "MeleeWeaponClass") },
			{ "Buff", new XmlWriteInfo("buff", "buffs", "buff") },
			{ "Perk", new XmlWriteInfo("perk", "perks", "perk") },
		};

		public static bool TryGetXmlNames(string modItemType, out string groupName, out string elementName)
		{
			if (ElementMapping.TryGetValue(modItemType, out var pair))
			{
				groupName = pair.GroupName;
				elementName = pair.ElementName;
				return true;
			}

			groupName = elementName = null;
			return false;
		}
	}

}
