using ModForge.Shared.Models.STORM.Rules;
using ModForge.Shared.Models.STORM.Selectors;
using ModForge.Shared.Models.STORM.Tasks;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ModForge.Shared.Models.STORM
{
	[XmlRoot("storm")]
	public class Storm
	{
		[XmlElement("common")]
		public Common Common { get; set; }
		[XmlElement("tasks")]
		public TaskCollection Tasks { get; set; }
		[XmlElement("customSelectors")]
		public CustomSelectors CustomSelectors { get; set; }
		[XmlElement("rules")]
		public RuleCollection Rules { get; set; }
	}
}
