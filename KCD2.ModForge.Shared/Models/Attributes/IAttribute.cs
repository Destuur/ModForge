namespace KCD2.ModForge.Shared.Models.Attributes
{
	public interface IAttribute
	{
		string Name { get; set; }
		object Value { get; set; }

		IAttribute DeepClone();
	}
}
