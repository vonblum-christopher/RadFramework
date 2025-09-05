using RadFramework.Libraries.Pipelines.Parameters;

namespace RadFramework.Libraries.Abstractions;

public interface IExtensionPipeline
{
    void Process(object input, ExtensionPipeContextBase pipeContext);
}

public interface IExtensionPipeline<TInput, TOutput> : IExtensionPipe 
{
    void Process(TInput input, ExtensionPipeContext<TOutput> pipeContext);
}