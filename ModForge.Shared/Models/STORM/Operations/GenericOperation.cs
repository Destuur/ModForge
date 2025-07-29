namespace ModForge.Shared.Models.STORM.Operations
{
	public class GenericOperation : IOperation
	{
		public string Name { get; set; }
		public Dictionary<string, string> Attributes { get; set; } = new();
		public List<GenericOperation> Children { get; set; } = new();

		public override string ToString()
		{
			return $"{Name} ({Attributes.Count} attrs, {Children.Count} children)";
		}
	}
}
