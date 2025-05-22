using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCD2.XML.Tool.Shared.Models
{
	public class ModDescription
	{
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string Author { get; set; } = string.Empty;
		public string Version { get; set; } = string.Empty;
		public DateTime CreatedOn { get; set; }
		public string ModId { get; set; } = string.Empty;
		public bool ModifiesLevel { get; set; }
		public List<string> SupportsVersions { get; set; } = new();
		public string ImagePath { get; set; } = string.Empty;
	}
}
