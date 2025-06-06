using KCD2.ModForge.Shared;
using KCD2.ModForge.Shared.Adapter;
using KCD2.ModForge.Shared.Factories;
using KCD2.ModForge.Shared.Models.ModItems;
using KCD2.ModForge.Shared.Services;
using System.Diagnostics;

namespace KCD2.ModForge.Test
{
	internal class Program
	{
		static void Main(string[] args)
		{
			//	var buffexclusive = new BuffExclusivityId("test", "1");
			//	var name = buffexclusive.Name;
			//	var value = buffexclusive.Value;

			//	// Wubba lubba dub dub
			//	var attribute1 = AttributeFactory.CreateAttribute("buff_params", "ewd*0.85,dew*2");
			//	var attribute2 = AttributeFactory.CreateAttribute("buff_ui_visibility_id", "1");
			//	var attribute3 = AttributeFactory.CreateAttribute("level", "12");
			//	var attribute4 = AttributeFactory.CreateAttribute("autolearnable", "true");

			//	var buffParams = new BuffParams("test", "ewd*0.85,dew*2");
			//	var buffParamsName = buffParams.Name;
			//	var buffParamsValue = buffParams.Value;

			//	var exportWatch = Stopwatch.StartNew();
			//	var userService = new UserConfigurationService();
			//	var gameDirectory = userService.Current.GameDirectory;
			//	var path = PathFactory.CreateLocalizationPath(gameDirectory, Language.German);

			//	var xmlPerkAdapter = new XmlAdapterOfT<Perk>(userService);
			//	var perks = xmlPerkAdapter.ReadModItems("").Result;

			//	var xmlBuffAdapter = new XmlAdapterOfT<Buff>(userService);
			//	var buffs = xmlBuffAdapter.ReadModItems("").Result;

			//	var localizationAdapter = new LocalizationAdapter(userService);


			//	var xmlToJsonService = new XmlToJsonService(xmlPerkAdapter, xmlBuffAdapter, localizationAdapter, new JsonAdapterOfT<Perk>("test"), new JsonAdapterOfT<Buff>("test"), userService);

			//	//ExportXmlToJson(xmlToJsonService);
			//	//exportWatch.Stop();

			//	var importWatch = Stopwatch.StartNew();
			//	ImportJson(xmlToJsonService);
			//	importWatch.Stop();

			//	Console.WriteLine(path);


			//	Console.ReadLine();
			//}

			//private static async void ImportJson(XmlToJsonService xmlToJsonService)
			//{
			//	var perks = await xmlToJsonService.ReadPerkJsonFile();
			//	var buffs = await xmlToJsonService.ReadBuffJsonFile();
			//}

			//private static async void ExportXmlToJson(XmlToJsonService xmlToJsonService)
			//{
			//	await xmlToJsonService.ConvertXmlToJsonAsync();
			//}
		}
	}
}
