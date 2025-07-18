using ModForge.Shared.Models.Abstractions;
using ModForge.Shared.Models.Mods;

namespace ModForge.Shared.Adapter
{
	public interface IModItemAdapter
	{
		public void WriteModItems(string modId, IEnumerable<IModItem> modItems);
		IList<IModItem> ReadModItems(IDataPoint dataPoint);
	}
}
