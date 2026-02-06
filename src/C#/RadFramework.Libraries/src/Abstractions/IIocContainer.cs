using System.Collections.Immutable;
using RadFramework.Libraries.Ioc.Base;
using RadFramework.Libraries.Ioc.Builder;

namespace RadFramework.Libraries.Abstractions;

public interface IIocContainer
{
    ImmutableList<IocDependency> ServiceList { get; }
    IImmutableDictionary<IIocKey, IocDependency> ServiceLookup { get; }
        
    bool HasService(IIocKey key);
        
    object Resolve(IIocKey key);

    object Activate(IIocKey key);
}