using System.Collections.Concurrent;
using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Ioc.Builder;

namespace RadFramework.Libraries.Ioc.Registrations;

public class InstanceContainerRegistry
{
    private IocBuilderRegistry builderRegistry;

    private ConcurrentDictionary<NamedIocKey, InstanceContainerBase> Containers;
    public InstanceContainerRegistry(IocBuilderRegistry builderRegistry)
    {
        this.builderRegistry = builderRegistry.Clone();
    }
}