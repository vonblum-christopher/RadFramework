using System.Collections.Immutable;
using RadFramework.Libraries.Ioc.Builder;

namespace RadFramework.Libraries.Abstractions;

public interface IIocContainer<TIocKey>
{
    ImmutableList<IocDependency<TIocKey>> ServiceList { get; }
    IImmutableDictionary<TIocKey, IocDependency<TIocKey>> ServiceLookup { get; }
        
    bool HasService(TIocKey key);
        
    object Resolve(TIocKey key);

    object Activate(TIocKey key);
}