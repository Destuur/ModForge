using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCD2.XML.Tool.Shared
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

	public enum PerkVisibility
	{
		SystemHidden,
		GameplayHidden,
		Visible,
		Obsolete,
	}
}
