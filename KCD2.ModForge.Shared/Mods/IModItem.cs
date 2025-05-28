using KCD2.ModForge.Shared.Models;
using KCD2.ModForge.Shared.Models.Attributes;

namespace KCD2.ModForge.Shared.Mods
{
	public interface IModItem
	{
		public string Id { get; }
		public string Path { get; }
		public IList<IAttribute> Attributes { get; }
		public IList<IModItem> ModItems { get; }
		public Localization Localization { get; }
	}
}
