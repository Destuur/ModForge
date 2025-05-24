using KCD2.XML.Tool.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCD2.XML.Tool.Shared.Services
{
	public class IconService
	{
		private List<Icon> icons = new();

		public void AddIcon(Icon icon)
		{
			icons.Add(icon);
		}

		public Icon GetIcon(string id)
		{
			foreach (var icon in icons)
			{
				if (icon.Id == id.Remove(id.Length - 5))
				{
					return icon;
				}
			}

			return null!;
		}
	}
}
