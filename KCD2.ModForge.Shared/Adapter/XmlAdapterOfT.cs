using KCD2.ModForge.Shared.Factories;
using KCD2.ModForge.Shared.Models.Attributes;
using KCD2.ModForge.Shared.Models.ModItems;
using KCD2.ModForge.Shared.Models.User;
using KCD2.ModForge.Shared.Services;
using System.IO.Compression;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;

namespace KCD2.ModForge.Shared.Adapter
{
	public class XmlAdapterOfT<T> : IModItemAdapter<T>
		where T : IModItem
	{
		private readonly UserConfigurationService userConfigurationService;

		public XmlAdapterOfT(UserConfigurationService userConfigurationService)
		{
			this.userConfigurationService = userConfigurationService;
		}

		public Task Initialize()
		{
			throw new NotImplementedException();
		}

		public Task Deinitialize()
		{
			throw new NotImplementedException();
		}

		public async Task<IList<T>> ReadAsync(string path)
		{
			var filePath = PathFactory.CreateTablesPath(userConfigurationService.Current!.GameDirectory);
			var modItemPath = string.Empty;
			var foundModItems = new List<T>();

			if (typeof(Perk).IsAssignableFrom(typeof(T)))
			{
				modItemPath = GetPerks(filePath, modItemPath, foundModItems);
			}

			if (typeof(Buff).IsAssignableFrom(typeof(T)))
			{
				modItemPath = GetBuffs(filePath, modItemPath, foundModItems);
			}

			return foundModItems;
		}

		private static string GetPerks(string filePath, string modItemPath, List<T> foundModItems)
		{
			using (FileStream zipToOpen = new FileStream(filePath, FileMode.Open))
			using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
			{
				foreach (var entry in archive.Entries)
				{
					if (entry.FullName.EndsWith(".tbl"))
					{
						continue;
					}

					if (entry.FullName.Contains("perk__combat") ||
						entry.FullName.Contains("perk__hardcore") ||
						entry.FullName.Contains("perk__kcd2"))
					{
						using (Stream stream = entry.Open())
						{
							try
							{
								modItemPath = Extensions.GetEntryPath(entry);

								XDocument doc = XDocument.Load(stream);

								foreach (var perkElement in doc.Descendants("perk"))
								{
									var modItem = ModItemFactory<T>.CreateModItem(perkElement, entry.FullName);

									//TODO: Platzhalter - löschen
									//if (modItem.Attributes.Count >= 11)
									//{
									//	continue;
									//}

									foundModItems.Add(modItem);
								}
							}
							catch (Exception ex)
							{
								Console.WriteLine($"Fehler beim Parsen von {entry.FullName}: {ex.Message}");
							}
						}
					}
				}
			}

			return modItemPath;
		}

		private static string GetBuffs(string filePath, string modItemPath, List<T> foundModItems)
		{
			using (FileStream zipToOpen = new FileStream(filePath, FileMode.Open))
			using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
			{
				foreach (var entry in archive.Entries)
				{
					if (entry.FullName.EndsWith(".tbl"))
					{
						continue;
					}

					if (entry.FullName.Contains("buff.xml") ||
						entry.FullName.Contains("buff__alchemy") ||
						entry.FullName.Contains("buff__perk") ||
						entry.FullName.Contains("buff__perk_hardcore") ||
						entry.FullName.Contains("buff__perk_kcd1"))
					{
						using (Stream stream = entry.Open())
						{
							try
							{
								modItemPath = Extensions.GetEntryPath(entry);

								XDocument doc = XDocument.Load(stream);

								foreach (var perkElement in doc.Descendants("buff"))
								{
									var modItem = ModItemFactory<T>.CreateModItem(perkElement, entry.FullName);

									//TODO: Platzhalter - löschen
									//if (modItem.Attributes.Count >= 11)
									//{
									//	continue;
									//}

									foundModItems.Add(modItem);
								}
							}
							catch (Exception ex)
							{
								Console.WriteLine($"Fehler beim Parsen von {entry.FullName}: {ex.Message}");
							}
						}
					}
				}
			}

			return modItemPath;
		}

		public Task<T> GetElement(string id)
		{
			throw new NotImplementedException();
		}

		public Task<bool> WriteElement(T modItem)
		{
			throw new NotImplementedException();
		}

		public Task<bool> WriteElements(IEnumerable<T> modItem)
		{
			throw new NotImplementedException();
		}
	}
}
