using RadFramework.Libraries.Pipelines.Parameters;

namespace RadFramework.Libraries.Abstractions;


public interface IExtensionPipe
{
    void Process(object input, ExtensionPipeContextBase pipeContext);
}
public interface IExtensionPipe<TInput, TOutput> : IExtensionPipe 
{
    void Process(TInput input, ExtensionPipeContext<TOutput> pipeContext);
}