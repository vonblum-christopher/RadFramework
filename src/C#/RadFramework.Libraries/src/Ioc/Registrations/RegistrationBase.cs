using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Ioc.Core;
using RadFramework.Libraries.Ioc.Factory;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc.Registrations
{
    public abstract class RegistrationBase : ICloneable<RegistrationBase>
    {
        public IocServiceRegistration IocServiceRegistration { get; set; }

        public virtual void Initialize(IocServiceRegistration serviceRegistration)
        {
            IocServiceRegistration = serviceRegistration;
        }
        
        public abstract object ResolveService(IocContainer container, IocServiceRegistration serviceRegistration);
        
        public abstract RegistrationBase Clone();
    }
}