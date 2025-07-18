namespace ModForge.Shared.Models.Abstractions
{
	public interface IDataPoint
	{
		public string Path { get; }
		public string Endpoint { get; }
		public Type Type { get; }
	}
}
