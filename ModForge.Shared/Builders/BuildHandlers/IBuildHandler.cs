namespace ModForge.Shared.Builders.BuildHandlers
{
	public interface IBuildHandler<TInput, TOutput> where TOutput : class
	{
		bool IsResponsible(TInput input);
		TOutput Handle(TInput input);
	}
}
