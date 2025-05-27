using KCD2.ModForge.Shared.Models.ModItems;
using KCD2.ModForge.Shared.Models.User;
using KCD2.ModForge.Shared.Mods;
using System.IO.Compression;
using System.Text.Json;
using System.Xml.Linq;

namespace KCD2.ModForge.Shared.Adapter
{
	public class XmlAdapter<T> : IModItemAdapter<T>
	{
		private readonly List<T> data;
		private readonly UserConfiguration userConfiguration;

		public XmlAdapter(UserConfiguration userConfiguration)
		{
			this.userConfiguration = userConfiguration;
		}

		public Task Initialize()
		{
			throw new NotImplementedException();
		}

		public Task Deinitialize()
		{
			throw new NotImplementedException();
		}

		public Task<IList<T>> GetAllElements()
		{
			string path = string.Empty;

			using (FileStream zipToOpen = new FileStream(tablePath, FileMode.Open))
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
								//path = Extensions.GetEntryPath(entry);

								XDocument doc = XDocument.Load(stream);

								foreach (var perkElement in doc.Descendants("perk"))
								{
									var perk = Perk.GetPerk(perkElement, entry.FullName);

									//TODO: Platzhalter - löschen
									if (perk.Attributes.Count >= 11)
									{
										continue;
									}

									perkService.AddPerk(perk);
									//modItems.Add(perk);
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
		}

		public Task<T> GetElement(string id)
		{
			throw new NotImplementedException();
		}

		public Task<bool> WriteElement(IModItem modItem)
		{
			throw new NotImplementedException();
		}

		public Task<bool> WriteElements(IEnumerable<IModItem> modItem)
		{
			throw new NotImplementedException();
		}
	}
}
