using KCD2.ModForge.Shared.Mods;

namespace KCD2.ModForge.Shared.Models.ModItems
{
	public class Localization : IModItem
	{
		public Localization(string id, string value, string path)
		{
			Id = id;
			Value = value;
			Path = path;
		}

		public string Id { get; private set; }
		public string Path { get; private set; }
		public string Value { get; set; }
		public string Attribute { get; set; } = string.Empty;


		internal static Localization GetLocalization(string id, string value, string path)
		{
			return new Localization(id, value, path);
		}
	}
}
