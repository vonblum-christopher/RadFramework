using System.Diagnostics;
using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc.Builder;

public class IocDependency : ICloneable<IocDependency>
{
    public IocKey Key { get; set; }
    public CachedType ImplementationType { get; set; }
    public InjectionOptions InjectionOptions { get; set; }
    public string IocLifecycle { get; set; }
    public Func<IocContainer, object> FactoryFunc { get; set; }
    
    public IocDependency Clone()
    {
        return new IocDependency
        {
            Key = Key,
            ImplementationType = ImplementationType,
            InjectionOptions = InjectionOptions.Clone(),
            IocLifecycle = IocLifecycle
        };
    }
}