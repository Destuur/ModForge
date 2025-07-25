using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM.Operations
{
	public class SetAttributeOperation : IOperation, ICustomOperation
	{
		public string Stat { get; set; }
		public string Skill { get; set; }
		public string ScaleWith { get; set; }
		public double? Value { get; set; }
		public double? MinValue { get; set; }
		public double? MaxValue { get; set; }
	}
}
