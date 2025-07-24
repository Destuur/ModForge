using Microsoft.AspNetCore.Components;

namespace ModForge.UI.Components.Overlays
{
	public partial class LoadingOverlay
	{
		[Parameter] public bool IsVisible { get; set; }

		private string[] Messages = new[]
		{
			"Reading XML Files…",
			"Skilling Perks…",
			"Applying Buffs…",
			"Yanking Pizzles…",
			"Saving Data…",
			"Sharpening Swords…",
			"Smashing Cuman Skulls…",
			"Mucking Out Stables…",
			"Challenging Hans Capon…",
			"Losing Dice Again…",
			"Boiling Herbs…",
			"Paying Off Guards…",
			"Hunting for Hares…",
			"Writing in Latin…",
			"Trying Not to Sin…",
			"Washing Off the Filth…",
			"Patching Reality…",
			"Rewriting Canon…",
			"Loading Immersion™…",
			"Simulating 1403…",
			"Defying Physics…",
			"Unpacking XML Wisdom…",
			"Polishing Horseshoes…",
			"Compiling Game Logic…",
			"Reading Between the Buffs…",
			"Aligning Chivalry Parameters…",
			"Rebalancing Numbskulls…",
			"Debugging Medieval AI…",
			"Seducing Bath Wenches…",
			"Shoveling Shit…",
			"Honoring Warhorse Studios…",
			"Defending Pebbles’ Honor…",
			"Moistening the Dry Devil…",
			"Staying Loyal to Theresa…",
			"Pawning Bianca’s Ring…"
		};
	}
}
