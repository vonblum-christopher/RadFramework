namespace RadFramework.Libraries.Patterns.Pipeline;

public abstract class ExtensionPipeBase<TInput, TOutput> : IExtensionPipe<TInput, TOutput>
{
    public virtual ExtensionPipeContext<TOutput> Process(TInput input, ExtensionPipeContext<TOutput> pipeContext)
    {
        return Process(input, pipeContext);
    }
    
    public abstract ExtensionPipeContextBase Process(object input, ExtensionPipeContextBase pipeContext);
}