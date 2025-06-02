using KCD2.ModForge.Shared.Models.Attributes;
using KCD2.ModForge.Shared.Models.ModItems;
using System.Linq.Expressions;
using System.Xml;
using System.Xml.Linq;

namespace KCD2.ModForge.Shared.Factories
{
	public static class ModItemFactory<T>
	{
		private static Func<string, IEnumerable<IAttribute>, T> BuildAttribute(string path, IEnumerable<IAttribute> attributes)
		{
			var type = typeof(T);

			var pathExpression = Expression.Parameter(typeof(string), nameof(path));
			var attributesExpression = Expression.Parameter(typeof(IEnumerable<IAttribute>), nameof(attributes));

			var constructor = type.GetConstructor(new[] { typeof(string), typeof(IEnumerable<IAttribute>) });

			if (constructor == null)
				throw new InvalidOperationException($"Kein passender Konstruktor in {type.Name} gefunden. Erwartet: (string, IEnumerable<IAttribute>)");

			var newExpression = Expression.New(constructor!, pathExpression, attributesExpression);

			var lambda = Expression.Lambda<Func<string, IEnumerable<IAttribute>, T>>(newExpression, pathExpression, attributesExpression);
			var func = lambda.Compile();

			return func!;
		}

		public static T CreateModItem(XElement element, string path)
		{
			try
			{
				IEnumerable<IAttribute> attributes = element.Attributes().Select(attr => AttributeFactory.CreateAttribute(attr.Name.LocalName, attr.Value));
				var modItem = BuildAttribute(path, attributes).Invoke(path, attributes);

				return modItem;
			}
			catch (Exception e)
			{
				throw new Exception();
			}
		}
	}
}
