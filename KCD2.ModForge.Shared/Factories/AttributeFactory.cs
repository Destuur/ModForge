using KCD2.ModForge.Shared.Models.Attributes;
using System.Linq.Expressions;
using System.Reflection;

namespace KCD2.ModForge.Shared.Factories
{
	// TODO: Factory fertig bauen
	public static class AttributeFactory
	{
		private static Func<string, object, IAttribute> BuildAttribute(string name, object value)
		{
			string typeName = string.Concat(
				name.Split('_').Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1))
			);
			// Das aktuelle Assembly (oder ein beliebiges anderes)
			var assembly = Assembly.GetExecutingAssembly(); // oder Assembly.Load("Dein.Assembly.Name")
															// Alle aktuell geladenen Assemblies durchsuchen
			var foundType = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(a => a.GetTypes())
				.FirstOrDefault(t => t.Name == typeName);

			var typeExpression = Expression.Parameter(typeof(string), nameof(name));
			var valueExpression = Expression.Parameter(typeof(object), nameof(value));

			var constructor = foundType.GetConstructors().FirstOrDefault(ctor => ctor.GetParameters().Length == 2);

			var newExpression = Expression.New(constructor!, typeExpression, valueExpression);

			var lambda = Expression.Lambda<Func<string, object, IAttribute>>(newExpression, typeExpression, valueExpression);
			var func = lambda.Compile();

			return func!;
		}

		public static IAttribute CreateAttribute(string name, object value)
		{
			return BuildAttribute(name, value).Invoke(name, value);
		}
	}
}
