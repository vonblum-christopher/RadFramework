using RadFramework.Libraries.Ioc;
using RadFramework.Libraries.Pipelines.Base;
using RadFramework.Libraries.Pipelines.Builder;

namespace RadFramework.Libraries.Pipelines;

public class ExtensionPipelineBase
{
    private IIocContainer container;
    public LinkedList<IExtensionPipe> pipes;

    public ExtensionPipelineBase(IEnumerable<PipeDefinition> pipeDefinitions, IIocContainer container)
    {
        this.container = container;
        this.pipes = new LinkedList<IExtensionPipe>(pipeDefinitions.Select(CreatePipe));
    }

    private IExtensionPipe CreatePipe(PipeDefinition def)
    {
        return (IExtensionPipe) container.Activate(def.Type);
    }
}