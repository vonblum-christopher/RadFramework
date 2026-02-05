using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Ioc;
using RadFramework.Libraries.Pipelines.Builder;

namespace RadFramework.Libraries.Pipelines.Base;

public class ExtensionPipelineBase
{
    private ITypeOnlyIocContainer container;
    public LinkedList<IExtensionPipe> pipes;

    public ExtensionPipelineBase(IEnumerable<PipeDefinition> pipeDefinitions, ITypeOnlyIocContainer container)
    {
        this.container = container;
        this.pipes = new LinkedList<IExtensionPipe>(pipeDefinitions.Select(CreatePipe));
    }

    private IExtensionPipe CreatePipe(PipeDefinition def)
    {
        return (IExtensionPipe) container.Activate(def.Type);
    }
}