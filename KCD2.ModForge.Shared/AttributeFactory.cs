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
