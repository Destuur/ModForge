using KCD2.XML.Tool.Shared.Mods;
using System.Xml.Linq;

namespace KCD2.XML.Tool.Shared.Models
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
