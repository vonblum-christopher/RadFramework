using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Ioc.Factory;
using RadFramework.Libraries.Ioc.Registrations;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc.Core;

public class IocServiceRegistration : ICloneable<IocServiceRegistration>
{
    public IocKey Key { get; set; }
    public InjectionOptions InjectionOptions { get; set; }
    public CachedType ImplementationType { get; set; }
    public string IocLifecycle { get; set; }
    public Func<Core.IocContainer, object> FactoryFunc { get; set; }
    
    
    public IocServiceRegistration Clone()
    {
        return new IocServiceRegistration()
        {
            Key = Key,
            ImplementationType = ImplementationType,
            InjectionOptions = InjectionOptions.Clone(),
            IocLifecycle = IocLifecycle
        };
    }
}