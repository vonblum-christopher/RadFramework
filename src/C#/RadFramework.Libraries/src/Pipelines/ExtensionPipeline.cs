using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Ioc;
using RadFramework.Libraries.Pipelines.Base;
using RadFramework.Libraries.Pipelines.Builder;
using RadFramework.Libraries.Pipelines.Parameters;

namespace RadFramework.Libraries.Pipelines;

public class ExtensionPipeline<TInput, TOutput> : ExtensionPipelineBase
{
    public ExtensionPipeline(PipelineBuilder builder, ITypeOnlyIocContainer container) : base(builder.Clone().PipeDefinitions, container)
    {
    }
    
    public TOutput Process(TInput input)
    {
        return Process(input, new ExtensionPipeContext<TOutput>()).ReturnValue;
    }
    
    public ExtensionPipeContext<TOutput> Process(TInput input, ExtensionPipeContext<TOutput> pipeContext)
    {
        foreach (IExtensionPipe pipe in pipes)
        {
            try
            {
                pipe.Process(input, pipeContext);
            }
            catch (Exception e)
            {
                pipeContext.Error(e);
            }
            
            if (pipeContext.ShouldReturn)
            {
                return pipeContext;
            }
        }

        return pipeContext;
    }
}