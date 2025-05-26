namespace KCD2.ModForge.Shared.Services
{
	public interface IFolderPickerService
	{
		Task<string?> PickFolderAsync();
	}
}
