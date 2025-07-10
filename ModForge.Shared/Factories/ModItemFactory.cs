﻿using ModForge.Shared.Models.Abstractions;
using System.Linq.Expressions;
using System.Xml;
using System.Xml.Linq;

namespace ModForge.Shared.Factories
{
	public static class ModItemFactory
	{
		private static Func<string, IEnumerable<IAttribute>, IModItem> BuildModItem(string path, Type type, IEnumerable<IAttribute> attributes)
		{
			var pathExpression = Expression.Parameter(typeof(string), nameof(path));
			var attributesExpression = Expression.Parameter(typeof(IEnumerable<IAttribute>), nameof(attributes));

			var constructor = type.GetConstructor(new[] { typeof(string), typeof(IEnumerable<IAttribute>) });

			if (constructor == null)
			{
				throw new InvalidOperationException($"Kein passender Konstruktor in {type.Name} gefunden. Erwartet: (string, IEnumerable<IAttribute>)");
			}

			var newExpression = Expression.New(constructor!, pathExpression, attributesExpression);

			var lambda = Expression.Lambda<Func<string, IEnumerable<IAttribute>, IModItem>>(newExpression, pathExpression, attributesExpression);
			var func = lambda.Compile();

			return func!;
		}

		public static IModItem CreateModItem(XElement element, Type type, string path)
		{
			try
			{
				IEnumerable<IAttribute> attributes = element.Attributes().Select(attr => AttributeFactory.CreateAttribute(attr.Name.LocalName, attr.Value));
				var modItem = BuildModItem(path, type, attributes).Invoke(path, attributes);

				return modItem;
			}
			catch (Exception e)
			{
				throw new Exception();
			}
		}
	}
}
