using ModForge.Shared.Models.Attributes;
using ModForge.Shared.Models.Localizations;

namespace ModForge.Shared.Models.ModItems
{
	public class MiscItem : IModItem
	{
		public MiscItem()
		{

		}

		public MiscItem(string id, string path, IList<string> linkedIds, IList<IAttribute> attributes, Localization localization)
		{
			Id = id;
			Path = path;
			LinkedIds = linkedIds;
			Attributes = attributes;
			Localization = localization;
		}

		public string Id { get; set; }
		public string Path { get; set; }
		public IList<string> LinkedIds { get; set; }
		public IList<IAttribute> Attributes { get; set; }
		public Localization Localization { get; set; }

		public IModItem GetDeepCopy(IModItem modItem)
		{
			return new MiscItem(modItem.Id, modItem.Path, modItem.LinkedIds, modItem.Attributes.Select(attr => attr.DeepClone()).ToList(), modItem.Localization.DeepClone());
		}
	}
}
