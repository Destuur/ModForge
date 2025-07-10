using ModForge.Shared.Models.Abstractions;

namespace ModForge.Shared.Models.Mods
{
	public class ModDescription
	{
		public ModDescription()
		{
			Color = GetRandomColor();
		}

		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string Author { get; set; } = string.Empty;
		public string ModVersion { get; set; } = string.Empty;
		public string CreatedOn { get; set; } = string.Empty;
		public string Id { get; set; } = string.Empty;
		public bool ModifiesLevel { get; set; }
		public List<string> SupportsGameVersions { get; set; } = new();
		public string ImagePath { get; set; } = string.Empty;
		public List<IModItem> ModItems { get; set; } = new();
		public string Link { get; set; }
		public string Color { get; set; }

		private string GetRandomColor()
		{
			var r = Random.Shared.Next(50, 200);
			var g = Random.Shared.Next(50, 200);
			var b = Random.Shared.Next(50, 200);

			return $"rgb({r}, {g}, {b})";
		}
	}
}
