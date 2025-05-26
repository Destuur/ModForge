using KCD2.ModForge.Shared.Models;

namespace KCD2.ModForge.Shared.Services
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
