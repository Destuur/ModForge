using ModForge.Shared.Models.Localizations;

namespace ModForge.Shared.Models.Abstractions
{
	public interface IModItem
	{
		public string Id { get; set; }
		public string IdKey { get; set; }
		public string Path { get; set; }
		public List<string> LinkedIds { get; set; }
		public List<IAttribute> Attributes { get; set; }
		public Localization Localization { get; set; }

		IModItem GetDeepCopy();
	}
}
