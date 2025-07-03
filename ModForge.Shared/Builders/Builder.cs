using Microsoft.Extensions.Logging;
using ModForge.Shared.Builders.BuildHandlers;

namespace ModForge.Shared.Builders
{
	public class Builder<TInput, TOutput> : IBuilder<TInput, TOutput> where TOutput : class
	{
		public Builder(ILogger<Builder<TInput, TOutput>> logger)
		{
			Logger = logger;
		}

		public List<IBuildHandler<TInput, TOutput>> Handlers { get; set; }
		public ILogger<Builder<TInput, TOutput>> Logger { get; set; }

		public TOutput Build(TInput input)
		{
			try
			{
				foreach (var handler in Handlers)
				{
					if (handler.IsResponsible(input))
					{
						return handler.Handle(input);
					}
				}
			}
			catch (Exception e)
			{
				Logger.LogError(e, $"Input '{input}' could not be built.");
			}

			return null;
		}
	}
}
