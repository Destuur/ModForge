namespace KCD2.ModForge.Shared.Adapter
{
	public interface IModItemAdapter<T>
	{
		Task Initialize();
		Task Deinitialize();
		Task<IList<T>> ReadAsync(string path);
		Task<T> GetModItem(string id);
		Task<bool> WriteElement(T modItem);
		Task<bool> WriteElements(IEnumerable<T> modItem);
	}
}
