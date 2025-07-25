using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM.Operations
{
	public class CustomOperation : ICustomOperation
	{
		public string Name { get; set; }
		public string Mode { get; set; }
		public string Comment { get; set; }
		public List<ModAttributeOperation> ModAttributes { get; set; } = new();
	}
}
