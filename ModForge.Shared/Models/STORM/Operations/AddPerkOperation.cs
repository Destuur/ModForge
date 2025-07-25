using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM.Operations
{
	public class AddPerkOperation : IOperation, ICustomOperation
	{
		public string Name { get; set; }
	}
}
