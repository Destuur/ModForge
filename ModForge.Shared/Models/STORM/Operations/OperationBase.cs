using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM.Operations
{
	[XmlInclude(typeof(SetAttributeOperation))]
	[XmlInclude(typeof(AddPerkOperation))]
	public abstract class OperationBase { }
}
