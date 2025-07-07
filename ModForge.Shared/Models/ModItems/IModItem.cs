using ModForge.Shared.Models.Attributes;
using ModForge.Shared.Models.Localizations;

namespace ModForge.Shared.Models.ModItems
{
	public interface IModItem
	{
		public string Id { get; set; }
		public string Path { get; set; }
		public List<string> LinkedIds { get; set; }
		public List<IAttribute> Attributes { get; set; }
		public Localization Localization { get; set; }

		IModItem GetDeepCopy();
	}
}
