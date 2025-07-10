using ModForge.Shared.Models.Abstractions;

namespace ModForge.Shared.Models.Data
{
	public class DataPoint : IDataPoint
	{
		public DataPoint(string path, string endpoint, Type type)
		{
			Path = path;
			Endpoint = endpoint;
			Type = type;
		}

		public string Path { get; }
		public string Endpoint { get; }
		public Type Type { get; }
	}
}
