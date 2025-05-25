using KCD2.XML.Tool.Shared.Mods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCD2.XML.Tool.Shared.Adapter
{
	public interface IModItemAdapter<T>
	{
		Task Initialize();
		Task Deinitialize();
		Task<List<T>> GetAllElements();
		Task<T> GetElement(string id);
		Task<bool> WriteMod(ModDescription mod);
		Task<bool> WriteModManifest(ModDescription modDescription);
	}

	public class JsonAdapter<T> : IModItemAdapter<T>
	{

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

		public Task<bool> WriteMod(ModDescription mod)
		{
			throw new NotImplementedException();
		}

		public Task<bool> WriteModManifest(ModDescription modDescription)
		{
			throw new NotImplementedException();
		}
	}
}
