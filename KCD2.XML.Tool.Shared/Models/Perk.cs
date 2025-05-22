using KCD2.XML.Tool.Shared.Mods;
using System.Xml.Linq;

namespace KCD2.XML.Tool.Shared.Models
{
	public class Perk : IModItem
	{
		public string Id { get; private set; } = string.Empty;
		public string Path { get; private set; } = string.Empty;
		public Dictionary<string, string> Attributes { get; set; } = new();
		public IEnumerable<IModItem>? Buffs { get; private set; }
		public IEnumerable<IModItem>? Localizations { get; private set; }

		public static Perk GetPerk(XElement element, string path)
		{
			var perk = new Perk();
			perk.Path = path;

			foreach (var attr in element.Attributes())
			{
				if (attr.Name == "perk_id")
				{
					perk.Id = attr.Value;
				}
				perk.Attributes[attr.Name.LocalName] = attr.Value;
			}
			return perk;
		}
	}
}
