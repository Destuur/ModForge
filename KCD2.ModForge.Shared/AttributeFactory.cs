using KCD2.ModForge.Shared.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCD2.ModForge.Shared
{
	// TODO: Factory fertig bauen
	public static class AttributeFactory
	{
		private static Func<string, object, ImmediateSubscriptionTrigger> NewMethod(string name, string value)
{
    string typeName = string.Concat(
        name.Split('-').Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1))
    );
    // Das aktuelle Assembly (oder ein beliebiges anderes)
    Assembly assembly = Assembly.GetExecutingAssembly(); // oder Assembly.Load("Dein.Assembly.Name")
    // Alle aktuell geladenen Assemblies durchsuchen
    Type? foundType = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(a => a.GetTypes())
        .FirstOrDefault(t => t.Name == typeName);

    var typeExpression = Expression.Parameter(typeof(string), nameof(name));
    var valueExpression = Expression.Parameter(typeof(object), nameof(value));

    var constructor = foundType!.GetConstructors().FirstOrDefault();

    var newExpression = Expression.New(constructor!, typeExpression, valueExpression);

    var lambda = Expression.Lambda<Func<string, object, ImmediateSubscriptionTrigger>>(newExpression, typeExpression, valueExpression);
    var func = lambda.Compile();

    return func!;
}
		
		public static IAttribute CreateAttribute(string type, object value)
		{
			return type switch
			{
				"autolearnable" => new Autolearnable(type, value),
				"buff_class_id" => new BuffClassId(type, value),
				"buff_exclusivity_id" => new BuffExclusivityId(type, value),
				"buff_id" => new BuffId(type, value),
				"buff_lifetime_id" => new BuffLifetimeId(type, value),
				"buff_name" => new BuffName(type, value),
				"buff_params" => new BuffParams(type, value),
				//"autolearnable" => new Autolearnable(type, value),
				//"autolearnable" => new Autolearnable(type, value),
				//"autolearnable" => new Autolearnable(type, value),
				//"autolearnable" => new Autolearnable(type, value),
				//"autolearnable" => new Autolearnable(type, value),
				//"autolearnable" => new Autolearnable(type, value),
				//"autolearnable" => new Autolearnable(type, value),
				//"autolearnable" => new Autolearnable(type, value),
				//"autolearnable" => new Autolearnable(type, value),
				//"autolearnable" => new Autolearnable(type, value),
				//"autolearnable" => new Autolearnable(type, value),
				//"autolearnable" => new Autolearnable(type, value),
				//"autolearnable" => new Autolearnable(type, value),
				//"autolearnable" => new Autolearnable(type, value),
				//"autolearnable" => new Autolearnable(type, value),
				//"autolearnable" => new Autolearnable(type, value),
				//"autolearnable" => new Autolearnable(type, value),
				//"autolearnable" => new Autolearnable(type, value),
				//"autolearnable" => new Autolearnable(type, value),
				//"autolearnable" => new Autolearnable(type, value),
				//"autolearnable" => new Autolearnable(type, value),
				//"autolearnable" => new Autolearnable(type, value),
				//"autolearnable" => new Autolearnable(type, value),
				_ => throw new ArgumentException("Attribute unkown")
			};
		}
	}
}
