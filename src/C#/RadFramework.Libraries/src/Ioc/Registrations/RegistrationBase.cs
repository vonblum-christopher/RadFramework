using RadFramework.Libraries.Ioc.Core;
using RadFramework.Libraries.Ioc.Factory;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc.Registrations
{
    public abstract class RegistrationBase : ICloneable
    {
        public InjectionOptions InjectionOptions { get; set; }
        public CachedType ImplementationType { get; set; }

        public abstract object ResolveService(IocContainer container);
        public abstract object Clone();
    }
}