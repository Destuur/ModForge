using ModForge.Shared.Models.STORM.Operations;

namespace ModForge.Shared.Models.STORM
{
	public static partial class OperationParser
	{
		public class GenericOperation : IOperation
		{
			public string ElementName { get; set; }
			public Dictionary<string, string> Attributes { get; set; } = new();
			public List<GenericOperation> Children { get; set; } = new();

			public override string ToString()
			{
				return $"{ElementName} ({Attributes.Count} attrs, {Children.Count} children)";
			}
		}
	}
}
