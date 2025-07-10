namespace ModForge.Shared.Models.Abstractions
{
	public interface IAttribute
	{
		string Name { get; set; }
		object Value { get; set; }

		IAttribute DeepClone();
	}
}
