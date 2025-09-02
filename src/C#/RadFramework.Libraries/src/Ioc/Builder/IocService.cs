using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Ioc.Factory;
using RadFramework.Libraries.Ioc.Registrations;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc.Core;

public class IocService : ICloneable<IocService>
{
    public IocKey Key { get; set; }
    public InjectionOptions InjectionOptions { get; set; }
    public CachedType ImplementationType { get; set; }
    public string IocMode { get; set; }
    
    public IocService Clone()
    {
        return new IocService()
        {
            Key = Key,
            ImplementationType = ImplementationType,
            InjectionOptions = InjectionOptions.Clone(),
            IocMode = IocMode
        };
    }
}