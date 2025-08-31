using RadFramework.Libraries.Ioc;

namespace RadFramework.Libraries.Patterns.Pipeline;

public class ExtensionPipeline<TInput, TOutput>
{
    private readonly IIocContainer _serviceProvider;
    public LinkedList<IExtensionPipe> pipes;

    public ExtensionPipeline(PipelineBuilder builder, IIocContainer serviceProvider)
    {
        _serviceProvider = serviceProvider;
        this.pipes = new LinkedList<IExtensionPipe>(builder.Definitions.Select(CreatePipe));
    }
        
    public ExtensionPipeline(IEnumerable<IExtensionPipe<TInput, TOutput>> pipes)
    {
        this.pipes = new LinkedList<IExtensionPipe>(pipes);
    }

    private IExtensionPipe<TInput, TOutput> CreatePipe(PipeDefinition def)
    {
        return (IExtensionPipe<TInput, TOutput>) _serviceProvider.Activate(def.Type);
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