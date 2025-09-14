using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Pipelines.Parameters;

namespace RadFramework.Libraries.Pipelines.Base;

public abstract class ExtensionPipeBase<TInput, TOutput> : IExtensionPipe<TInput, TOutput>, IExtensionPipe
{
    public abstract void Process(TInput input, ExtensionPipeContext<TOutput> pipeContext);

    public void Process(object input, ExtensionPipeContextBase pipeContext)
    {
        Process((TInput)input, (ExtensionPipeContext<TOutput>)pipeContext);
    }
}