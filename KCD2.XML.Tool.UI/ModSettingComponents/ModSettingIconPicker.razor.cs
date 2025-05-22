using KCD2.XML.Tool.Shared.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCD2.XML.Tool.UI.ModSettingComponents
{
	public partial class ModSettingIconPicker
	{
		private string path = string.Empty;

		[Inject]
		public ModService? ModService { get; set; }

		public async Task SaveMod()
		{
			await Task.Yield();

			if (ModService is null)
			{
				return;
			}

			if (string.IsNullOrEmpty(path))
			{
				ModService.AddModIcon("images/Icons/crime_investigating.png");
			}

			await ModService.AddModIcon(path);
		}
	}
}
