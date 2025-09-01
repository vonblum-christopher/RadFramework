using RadFramework.Libraries.Ioc.Factory;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc.Registrations
{
    public abstract class RegistrationBase
    {
        public InjectionOptions InjectionOptions { get; set; }
        public CachedType ImplementationType { get; set; }

        public abstract object ResolveService();
    }
}