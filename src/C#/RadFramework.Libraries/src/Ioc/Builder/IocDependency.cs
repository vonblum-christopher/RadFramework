using System.Diagnostics;
using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc.Builder;

public class IocDependency<TIocKey> : ICloneable<IocDependency<TIocKey>>
{
    public TIocKey Key { get; set; }
    public CachedType ImplementationType { get; set; }
    public InjectionOptions InjectionOptions { get; set; }
    public string IocLifecycle { get; set; }
    public Func<IocDependency<TIocKey>, TypeOnlyIocContainer, object> FactoryFunc { get; set; }
    
    public IocDependency<TIocKey> Clone()
    {
        return new IocDependency<TIocKey>
        {
            Key = Key,
            ImplementationType = ImplementationType,
            InjectionOptions = InjectionOptions.Clone(),
            IocLifecycle = IocLifecycle
        };
    }
}