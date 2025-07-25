namespace ModForge.Shared.Models.STORM.Operations
{
	public class ModAttributeOperation : IOperation, ICustomOperation
	{
		public string Stat { get; set; }
		public double? MinMod { get; set; }
		public double? MaxMod { get; set; }
	}
}
