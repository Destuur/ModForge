using ModForge.Shared.Models.Abstractions;
using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM.Operations
{
	public class OperationContainer
	{
		[XmlElement("setAttribute", typeof(SetAttributeOperation))]
		[XmlElement("modAttribute", typeof(ModAttributeOperation))]
		[XmlElement("addPerk", typeof(AddPerkOperation))]
		public List<IOperation> Operations { get; set; } = new();
	}

	public class CustomOperationContainer
	{
		[XmlElement("setAttribute", typeof(SetAttributeOperation))]
		[XmlElement("modAttribute", typeof(ModAttributeOperation))]
		[XmlElement("addPerk", typeof(AddPerkOperation))]
		public List<ICustomOperation> CustomOperations { get; set; } = new();
	}

	public class CustomOperation : ICustomOperation
	{
		[XmlAttribute("name")]
		public string Name { get; set; }

		[XmlAttribute("mode")]
		public string Mode { get; set; }

		[XmlElement("modAttribute")]
		public List<ModAttributeOperation> ModAttributes { get; set; }
	}

	public class ModAttributeOperation : IOperation, ICustomOperation
	{
		[XmlAttribute("stat")]
		public string Stat { get; set; }
		[XmlAttribute("minMod")]
		public double MinMod { get; set; }
		[XmlAttribute("maxMod")]
		public double MaxMod { get; set; }
	}
}
