using KCD2.ModForge.Shared.Models.Attributes;
using KCD2.ModForge.Shared.Models.Localizations;

namespace KCD2.ModForge.Shared.Models.ModItems
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
