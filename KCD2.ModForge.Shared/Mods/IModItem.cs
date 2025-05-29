using KCD2.ModForge.Shared.Models;
using KCD2.ModForge.Shared.Models.Attributes;

namespace KCD2.ModForge.Shared.Mods
{
	public interface IModItem
	{
		public string Id { get; set; }
		public string Path { get; set; }
		public IList<IAttribute> Attributes { get; set; }
		public IList<IModItem> ModItems { get; set; }
		public Localization Localization { get; set; }
	}
}
