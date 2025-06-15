namespace ModForge.Shared
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

	public enum BuffFamily
	{
		AesopPotion = 1,
		Antidote = 2,
		AquaVitalisPotion = 3,
		ArtemisiaPotion = 4,
		BanePotion = 5,
		BardPotion = 6,
		BowmansBrew = 7,
		BucksBloodPotion = 8,
		ChamomileDecoction = 9,
		CockerelPotion = 10,
		DollmakerPotion = 11,
		Embrocation = 12,
		HairODogPotion = 13,
		LullabayPotion = 14,
		MarigoldDecoction = 15,
		NighthawkPotion = 16,
		PadfootPotion = 17,
		PainkillerPotion = 18,
		SavegamePotion = 19,
		FoodAgiExpMultiplier = 20,
		FoodStrExpMultiplier = 21,
		FoodVitExpMultiplier = 22,
		AlcoholCraving = 23,
		FoodPoisoning = 24,
		PainterMedicine = 25
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

	public enum Visibility
	{
		SystemHidden,
		GameplayHidden,
		Visible,
		Obsolete,
	}

	public enum SkillSelector
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

	public enum StatSelector
	{
		Strength,
		Agility,
		Vitality,
		Speech,
		Prestige = 9
	}

	public enum ExcludeInGameMode
	{
		normal = 1,
		hardcore = 2
	}

	public enum MathOperation
	{
		AddAbsolute,      // +
		SubtractAbsolute,      // -
		SetAbsolute,      // =
		AddRelativeToBase,  // *
		MultiplyCurrent,  // %
		Minimum,         // <
		Maximum,         // >
		NegateRelativeToValue     // !
	}
}
