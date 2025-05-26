namespace KCD2.ModForge.Shared.Mods
{
	public class ModDescription
	{
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string Author { get; set; } = string.Empty;
		public string ModVersion { get; set; } = string.Empty;
		public string CreatedOn { get; set; } = string.Empty;
		public string ModId { get; set; } = string.Empty;
		public bool ModifiesLevel { get; set; }
		public List<string> SupportsGameVersions { get; set; } = new();
		public string ImagePath { get; set; } = string.Empty;
		public List<IModItem> ModItems { get; set; } = new();
	}
}
