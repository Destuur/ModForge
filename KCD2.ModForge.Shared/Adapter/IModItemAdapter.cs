using KCD2.ModForge.Shared.Models.Data;
using KCD2.ModForge.Shared.Models.ModItems;
using KCD2.ModForge.Shared.Models.Mods;

namespace KCD2.ModForge.Shared.Adapter
{
	public interface IModItemAdapter
	{
		public void WriteModItems(string modId, IEnumerable<IModItem> modItems);
		IList<IModItem> ReadModItems(IDataPoint dataPoint);
	}
}
