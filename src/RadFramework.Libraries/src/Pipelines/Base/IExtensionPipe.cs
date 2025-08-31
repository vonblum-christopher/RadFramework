namespace RadFramework.Libraries.Patterns.Pipeline;


public interface IExtensionPipe
{
    ExtensionPipeContextBase Process(object input, ExtensionPipeContextBase pipeContext);
}
public interface IExtensionPipe<TInput, TOutput> : IExtensionPipe 
{
    ExtensionPipeContext<TOutput> Process(TInput input, ExtensionPipeContext<TOutput> pipeContext);
}