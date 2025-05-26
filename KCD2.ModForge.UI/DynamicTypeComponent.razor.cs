using Microsoft.AspNetCore.Components;

namespace KCD2.ModForge.UI
{
	public partial class DynamicTypeComponent
	{
		private static List<Type>? types;
		private Type? componentType;
		private RenderFragment? renderFragment;
		private RenderFragment? innerRenderFragment;

		private object? renderObject;
		private string? prefix;
		private string suffix = "Component";

		[Parameter]
		public object? RenderObject
		{
			get
			{
				return renderObject;
			}
			set
			{
				renderObject = value;
				Reload();
			}
		}

		[Parameter]
		public string? Prefix
		{
			get
			{
				return prefix;
			}
			set
			{
				prefix = value;
				Reload();
			}
		}

		[Parameter]
		public string Suffix
		{
			get
			{
				return suffix;
			}
			set
			{
				suffix = value;
				Reload();
			}
		}

		[Parameter] public bool UseStaticTypesList { get; set; } = true;
		[Parameter] public IDictionary<string, object>? Parameters { get; set; }

		private string GetGenericTypeName(Type type)
		{
			if (type.IsGenericType == false)
			{
				return type.Name;
			}
			else
			{
				var typeName = type.Name;
				typeName = typeName.Remove(typeName.IndexOf('`'));
				foreach (var innerType in type.GetGenericArguments())
				{
					typeName += GetGenericTypeName(innerType);
				}
				return typeName;
			}
		}

		public static void InitializeStaticList()
		{
			if (types == null)
			{
				types = AppDomain.CurrentDomain.GetAssemblies()
					.SelectMany(x => x.GetTypes()).ToList();
			}
		}

		protected override void OnInitialized()
		{
			base.OnInitialized();
			Reload();
		}

		protected void Reload()
		{
			if (RenderObject is null)
			{
				return;
			}

			var renderObjectTyp = RenderObject.GetType();
			string renderObjectTypName = GetGenericTypeName(renderObjectTyp);

			if (UseStaticTypesList)
			{
				InitializeStaticList();
				componentType = types!.Find(x => x.Name == $"{Prefix}{renderObjectTypName}{Suffix}");
			}
			else
			{
				componentType = AppDomain.CurrentDomain.GetAssemblies()
					.SelectMany(x => x.GetTypes())
					.FirstOrDefault(x => x.Name == $"{Prefix}{renderObjectTypName}{Suffix}");
			}

			innerRenderFragment = (builder) =>
			{
				builder.OpenComponent(0, typeof(DynamicComponent));
				builder.AddAttribute(1, "Type", componentType);
				builder.AddAttribute(2, "Parameters", Parameters);
				builder.CloseComponent();
			};

			renderFragment = (builder) =>
			{
				builder.OpenComponent(0, typeof(CascadingValue<>).MakeGenericType(renderObjectTyp));
				builder.AddAttribute(1, "Value", RenderObject);
				builder.AddAttribute(2, "ChildContent", innerRenderFragment);
				builder.CloseComponent();
			};
		}
	}
}
