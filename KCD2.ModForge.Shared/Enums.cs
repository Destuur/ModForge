namespace KCD2.ModForge.Shared
{
	public enum Language
	{
		NotValid,
		Chineses,
		Chineset,
		Czech,
		English,
		French,
		German,
		Italian,
		Japanese,
		Korean,
		Polish,
		Portuguese,
		Russian,
		Spanish,
		Turkish,
		Ukrainian
	}

	public enum BuffAiTag
	{
		alcohol_mood,
		alcohol_drunk,
		alcohol_blackout,
		poison,
		bleed,
		sleep,
		overeat,
		injury,
		savegame
	}

	public enum BuffExclusivity
	{
		NoExclusivity,
		IdExclusive,
		ModExclusive,
		ClassExclusive,
		TargetExclusive
	}

	public enum BuffLifetime
	{
		ShortTerm,
		LongTerm
	}

	public enum BuffUiType
	{
		Default,
		Buff,
		Debuff,
		InjuryDebuff,
		Perk
	}

	public enum BuffUiVisibility
	{
		None,
		HUD,
		Inventory,
		All
	}

	public enum BuffClass
	{
		SystemBuff = 1,
		WeaponSkillBuff = 2,
		PerkBuff = 4,
		Injury = 5,
		Heal = 6,
		Poison = 7,
		Perception = 8,
		Overeat = 9,
		Alcohol = 10,
		ItemBuff = 12,
		Potion = 13,
		FoodPoison = 14,
		ScriptSystem = 15,
		Unconsciousness = 16,
		Hangover = 17,
		Satisfaction = 18,
		Perfume = 19,
		Punishment = 20,
		ForcedDrunkeness = 21
	}

	public enum PerkVisibility
	{
		SystemHidden,
		GameplayHidden,
		Visible,
		Obsolete,
	}

	public enum SkillType
	{
		Stealth = 0,
		HorseRiding = 1,
		Fencing = 2,
		Thievery = 4,
		Alchemy = 6,
		Craftmanship = 8,
		Drinking = 13,
		Survival = 14,
		Defense = 15,
		WeaponSword = 16,
		HeavyWeapons = 17,
		Marksmanship = 19,
		WeaponShield = 20,
		WeaponLarge = 23,
		WeaponUnarmed = 24,
		Scholarship = 26,
	}

	public enum StatType
	{
		Strength,
		Agility,
		Vitality,
		Speech,
		Prestige = 9
	}

	public enum GameMode
	{
		normal = 1,
		hardcore = 2
	}
}
