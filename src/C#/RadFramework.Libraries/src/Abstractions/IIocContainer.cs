using System.Collections.Immutable;
using RadFramework.Libraries.Ioc.Builder;

namespace RadFramework.Libraries.Abstractions;

public interface IIocContainer<TIocKey>
{
    ImmutableList<IocDependency> ServiceList { get; }
    IImmutableDictionary<TIocKey, IocDependency> ServiceLookup { get; }

    bool HasService<TService>();
    bool HasService(Type t);
        
    bool HasService(TIocKey key);
        
    TService Resolve<TService>();
    object Resolve(Type tInterface);
    object Resolve(TIocKey key);

    TService Activate<TService>();
    object Activate(Type t);
    object Activate(TIocKey key);
}