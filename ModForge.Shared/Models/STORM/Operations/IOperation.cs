using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM.Operations
{
	[XmlInclude(typeof(SetAttributeOperation))]
	[XmlInclude(typeof(ModAttributeOperation))]
	[XmlInclude(typeof(AddPerkOperation))]
	public interface IOperation { }

	[XmlInclude(typeof(SetAttributeOperation))]
	[XmlInclude(typeof(ModAttributeOperation))]
	[XmlInclude(typeof(AddPerkOperation))]
	public interface ICustomOperation { }
}
