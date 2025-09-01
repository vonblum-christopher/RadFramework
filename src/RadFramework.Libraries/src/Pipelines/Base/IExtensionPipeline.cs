namespace RadFramework.Libraries.Pipelines.Base;

public interface IExtensionPipeline
{
    void Process(object input, ExtensionPipeContextBase pipeContext);
}

public interface IExtensionPipeline<TInput, TOutput> : IExtensionPipe 
{
    void Process(TInput input, ExtensionPipeContext<TOutput> pipeContext);
}