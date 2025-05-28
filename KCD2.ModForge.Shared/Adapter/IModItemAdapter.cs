using KCD2.ModForge.Shared.Mods;
using KCD2.ModForge.Shared.Services;

namespace KCD2.ModForge.Shared.Adapter
{
	public interface IModItemAdapter<T>
	{
		Task Initialize();
		Task Deinitialize();
		Task<IList<T>> ReadAsync();
		Task<T> GetElement(string id);
		Task<bool> WriteElement(IModItem modItem);
		Task<bool> WriteElements(IEnumerable<IModItem> modItem);
	}
}
