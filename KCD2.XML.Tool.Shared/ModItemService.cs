using KCD2.XML.Tool.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCD2.XML.Tool.Shared
{
	public class ModItemService
	{
		private List<IModItem> modItems = new();

		public IEnumerable<IModItem> GetAll()
		{
			return modItems!;
		}

		public bool AddItem(IModItem item)
		{
			if (item is null)
			{
				return false;
			}

			if (modItems.Contains(item))
			{
				return false;
			}

			modItems.Add(item);
			return true;
		}
	}
}
