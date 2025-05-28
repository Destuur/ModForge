using KCD2.ModForge.Shared;
using KCD2.ModForge.Shared.Models.Attributes;

namespace KCD2.ModForge.Test
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var buffexclusive = new BuffExclusivityId("test", "1");
			var name = buffexclusive.Name;
			var value = buffexclusive.Value;

			// Wubba lubba dub dub
			var attribute1 = AttributeFactory.CreateAttribute("buff_params", "ewd*0.85,dew*2");
			var attribute2 = AttributeFactory.CreateAttribute("buff_ui_visibility_id", "1");
			var attribute3 = AttributeFactory.CreateAttribute("level", "12");
			var attribute4 = AttributeFactory.CreateAttribute("autolearnable", "true");

			var buffParams = new BuffParams("test", "ewd*0.85,dew*2");
			var buffParamsName = buffParams.Name;
			var buffParamsValue = buffParams.Value;


			Console.ReadLine();
		}
	}
}
