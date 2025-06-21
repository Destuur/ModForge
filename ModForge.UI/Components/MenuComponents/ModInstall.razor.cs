﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using ModForge.Shared.Services;
using MudBlazor;

namespace ModForge.UI.Components.MenuComponents
{
	public partial class ModInstall
	{
		[Inject]
		public NavigationManager Navigation { get; set; }
		[Inject]
		public ModService ModService { get; set; }
		[Parameter]
		public EventCallback<Type> ChangeChildContent { get; set; }

		//TODO: Update mit Möglichkeit Thumbnails in der Mod zu hinterlegen!
		private const string DefaultDragClass = "relative rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full";
		private string _dragClass = DefaultDragClass;
		private readonly List<string> _fileNames = new();
		private MudFileUpload<IReadOnlyList<IBrowserFile>>? _fileUpload;

		private async Task ClearAsync()
		{
			await (_fileUpload?.ClearAsync() ?? Task.CompletedTask);
			_fileNames.Clear();
			ClearDragClass();
		}

		private Task OpenFilePickerAsync()
			=> _fileUpload?.OpenFilePickerAsync() ?? Task.CompletedTask;

		private void OnInputFileChanged(InputFileChangeEventArgs e)
		{
			ClearDragClass();
			var files = e.GetMultipleFiles();
			foreach (var file in files)
			{
				_fileNames.Add(file.Name);
			}
		}

		private void Upload()
		{
			// Upload the files here			
		}

		private void SetDragClass()
			=> _dragClass = $"{DefaultDragClass} mud-border-primary";

		private void ClearDragClass()
			=> _dragClass = DefaultDragClass;
	}
}
