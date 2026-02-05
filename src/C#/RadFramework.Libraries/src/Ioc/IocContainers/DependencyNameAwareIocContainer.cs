using System.Collections.Immutable;
using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Ioc.Builder;

namespace RadFramework.Libraries.Ioc;

public class DependencyNameAwareIocContainer : IIocContainer<NamedIocKey>
{
    public ImmutableList<IocDependency> ServiceList { get; }
    public IImmutableDictionary<NamedIocKey, IocDependency> ServiceLookup { get; }
    public bool HasService(NamedIocKey key)
    {
        throw new NotImplementedException();
    }

    public object Resolve(NamedIocKey key)
    {
        throw new NotImplementedException();
    }

    public object Activate(NamedIocKey key)
    {
        throw new NotImplementedException();
    }
}