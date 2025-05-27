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

			var attribute = AttributeFactory.CreateAttribute("buff_params", "ewd*0.85,dew*2");

			var buffParams = new BuffParams("test", "ewd*0.85,dew*2");
			var buffParamsName = buffParams.Name;
			var buffParamsValue = buffParams.Value;


			Console.ReadLine();
		}
	}
}
