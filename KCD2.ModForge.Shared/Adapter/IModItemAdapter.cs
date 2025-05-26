using KCD2.ModForge.Shared.Mods;
using System.Text.Json;

namespace KCD2.ModForge.Shared.Adapter
{
	public interface IModItemAdapter<T>
	{
		Task Initialize();
		Task Deinitialize();
		Task<List<T>> GetAllElements();
		Task<T> GetElement(string id);
		Task<bool> WriteElement(IModItem modItem);
		Task<bool> WriteElements(IEnumerable<IModItem> modItem);
	}

	public class JsonAdapter<T> : IModItemAdapter<T>
	{
		private readonly List<T> data;

		public JsonAdapter(string filePath)
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

		public Task<List<T>> GetAllElements()
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

	public class XmlAdapter<T> : IModItemAdapter<T>
	{
		private readonly List<T> data;

		public XmlAdapter(string filePath)
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

		public Task<List<T>> GetAllElements()
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
