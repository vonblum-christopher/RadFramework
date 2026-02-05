using RadFramework.Libraries.Ioc.Builder;

namespace RadFramework.Libraries.Ioc.Registrations;

public class InstanceContainerRegistry : IocBuilderRegistry
{
    private IocBuilderRegistry builderRegistry;

    public InstanceContainerRegistry(IocBuilderRegistry builderRegistry)
    {
        this.builderRegistry = builderRegistry.Clone();
    }
    
    
}