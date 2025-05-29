using KCD2.ModForge.Shared.Models.Attributes;

namespace KCD2.ModForge.Shared.Factories
{
	// TODO: Factory fertig bauen
	public static class AttributeFactory
	{
		public static IAttribute CreateAttribute(string name, object value)
		{
			string typeName = string.Concat(name.Split('_').Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1)));

			var foundType = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(a => a.GetTypes())
				.FirstOrDefault(t => t.Name == typeName && typeof(IAttribute).IsAssignableFrom(t));

			if (foundType == null)
			{
				throw new InvalidOperationException($"Attribute type '{typeName}' not found.");
			}

			var instance = Activator.CreateInstance(foundType, name, value);

			if (instance is IAttribute attr)
			{
				return attr;
			}

			throw new InvalidOperationException($"Instance of type '{typeName}' is not an IAttribute.");
		}
	}
}
