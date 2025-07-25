using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM.Operations
{
	public class SetAttributeOperation : IOperation, ICustomOperation
	{
		[XmlAttribute("stat")]
		public string Stat { get; set; }
		[XmlAttribute("skill")]
		public string Skill { get; set; }
		[XmlAttribute("scaleWith")]
		public string ScaleWith { get; set; }
		[XmlAttribute("value")]
		public string Value { get; set; }
		[XmlAttribute("minValue")]
		public string MinValue { get; set; }
		[XmlAttribute("maxValue")]
		public string MaxValue { get; set; }
	}
}
