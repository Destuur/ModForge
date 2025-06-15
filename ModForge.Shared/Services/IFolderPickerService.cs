namespace ModForge.Shared.Services
{
	public interface IFolderPickerService
	{
		Task<string?> PickFolderAsync();
	}
}
