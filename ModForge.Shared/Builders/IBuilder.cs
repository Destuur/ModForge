using ModForge.Shared.Builders.BuildHandlers;
using ModForge.Shared.Models.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace ModForge.Shared.Builders
{
	public interface IBuilder<TInput, TOutput> where TOutput : class
	{
		List<IBuildHandler<TInput, TOutput>> Handlers { get; set; }

		TOutput Build(TInput input);
	}
}
