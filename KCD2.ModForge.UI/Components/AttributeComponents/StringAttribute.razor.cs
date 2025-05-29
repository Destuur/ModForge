using KCD2.ModForge.Shared.Models.Attributes;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCD2.ModForge.UI.Components.AttributeComponents
{
	public partial class StringAttribute
	{
		[CascadingParameter]
		public IAttribute Attribute { get; set; }
	}
}
