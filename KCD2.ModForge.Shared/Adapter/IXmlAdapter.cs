using KCD2.ModForge.Shared.Mods;

namespace KCD2.ModForge.Shared.Adapter
{
	public interface IXmlAdapter
	{
		Task Initialize();
		Task Deinitialize();
		Task<List<IModItem>> GetModItems();
		Task<IModItem> GetModItem(string id);
		Task<bool> WriteModItems(ModDescription mod);
		Task<bool> WriteModManifest(ModDescription modDescription);
	}
}
