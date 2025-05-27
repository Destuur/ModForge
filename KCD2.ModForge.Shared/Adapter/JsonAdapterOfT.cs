using KCD2.ModForge.Shared.Mods;
using System.Text.Json;

namespace KCD2.ModForge.Shared.Adapter
{
	public class JsonAdapterOfT<T> : IModItemAdapter<T>
	{
		private readonly List<T> data;

		public JsonAdapterOfT(string filePath)
		{
			var json = File.ReadAllText(filePath);
			data = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
		}

		public Task Initialize()
		{
			throw new NotImplementedException();
		}
		public Task Deinitialize()
		{
			throw new NotImplementedException();
		}

		public Task<IList<T>> GetAllElements()
		{
			throw new NotImplementedException();
		}

		public Task<T> GetElement(string id)
		{
			throw new NotImplementedException();
		}

		public Task<bool> WriteElement(IModItem modItem)
		{
			throw new NotImplementedException();
		}

		public Task<bool> WriteElements(IEnumerable<IModItem> modItem)
		{
			throw new NotImplementedException();
		}
	}
}
