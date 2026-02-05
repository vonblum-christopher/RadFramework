using System.Collections.Concurrent;
using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Ioc.Builder;

namespace RadFramework.Libraries.Ioc.Registrations;

public class InstanceContainerRegistry<TIocKey> where TIocKey : ICloneable<TIocKey>
{
    private IocBuilderRegistry<TIocKey> builderRegistry;

    private ConcurrentDictionary<NamedIocKey, InstanceContainerBase<TIocKey>> Containers;
    public InstanceContainerRegistry(IocBuilderRegistry<TIocKey> builderRegistry)
    {
        this.builderRegistry = builderRegistry.Clone();
    }
}