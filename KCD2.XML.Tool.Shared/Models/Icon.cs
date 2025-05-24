using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCD2.XML.Tool.Shared.Models
{
	public class Icon
	{
		public string Path { get; set; } = string.Empty;
		public string Id { get; set; } = string.Empty;
		public byte[] Image { get; set; } = [];
	}
}
