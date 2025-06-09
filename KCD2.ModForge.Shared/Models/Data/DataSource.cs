using KCD2.ModForge.Shared.Adapter;
using KCD2.ModForge.Shared.Models.ModItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCD2.ModForge.Shared.Models.Data
{
	public class DataSource
	{
		private readonly IModItemAdapter adapter;

		public DataSource(IModItemAdapter adapter)
		{
			this.adapter = adapter;
		}

		public IList<IModItem> ReadModItems(IDataPoint dataPoint)
		{
			if (dataPoint is null)
			{
				return new List<IModItem>();
			}
			return adapter.ReadModItems(dataPoint);
		}

		public void WriteModItems(IList<IModItem> modItems)
		{
			if (modItems is null)
			{
				return;
			}
			adapter.WriteModItems(modItems);
		}
	}
}
