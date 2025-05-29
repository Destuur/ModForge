namespace KCD2.ModForge.Shared.Models.Attributes
{
	public interface IAttribute
	{
		string Name { get; }
		object Value { get; set; }
	}
}
