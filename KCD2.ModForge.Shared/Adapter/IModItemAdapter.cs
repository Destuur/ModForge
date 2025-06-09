using KCD2.ModForge.Shared.Models.Data;
using KCD2.ModForge.Shared.Models.ModItems;

namespace KCD2.ModForge.Shared.Adapter
{
	public interface IModItemAdapter
	{
		void WriteModItems(IEnumerable<IModItem> modItem);
		IList<IModItem> ReadModItems(IDataPoint dataPoint);
	}
}
