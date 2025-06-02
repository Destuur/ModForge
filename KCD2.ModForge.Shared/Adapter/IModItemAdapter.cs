namespace KCD2.ModForge.Shared.Adapter
{
	public interface IModItemAdapter<T>
	{
		void Initialize();
		void Deinitialize();
		IList<T> ReadModItems(string path);
		T GetModItem(string id);
		bool WriteModItem(T modItem);
		bool WriteModItems(IEnumerable<T> modItem);
	}
}
