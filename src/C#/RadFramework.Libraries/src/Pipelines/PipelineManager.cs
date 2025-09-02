using System.Collections.Concurrent;
using RadFramework.Libraries.Ioc;
using RadFramework.Libraries.Ioc.Core;
using RadFramework.Libraries.Pipelines.Base;
using RadFramework.Libraries.Pipelines.Builder;
using IocContainer = RadFramework.Libraries.Ioc.Core.IocContainer;

namespace RadFramework.Libraries.Pipelines;

public class PipelineManager
{
    private readonly IocContainer container;

    private readonly object pipelineRegistryLock = new object();
    
    private readonly ConcurrentDictionary<IocKey, IExtensionPipeline> pipelineRegistry = new ();
    
    public PipelineManager(IocContainer container)
    {
        this.container = container;
    }
    
    public void RegisterPipeline<TPipeline>(PipelineBuilder builder)
        where TPipeline : IExtensionPipeline
    {
        container.RegisterSingletonInstance<TPipeline>(new ExtensionPipelineBase(builder.Definitions, container));
    }

    public TPipeline CreateRuntimePipeline<TPipeline>()
    {
        return container.Resolve<TPipeline>();
    }
}