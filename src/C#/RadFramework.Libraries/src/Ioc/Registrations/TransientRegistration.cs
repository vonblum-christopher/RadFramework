using System.Collections.Concurrent;
using RadFramework.Libraries.Ioc.Builder;
using RadFramework.Libraries.Ioc.ConstructionMethodBuilders;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc.Registrations
{
    public class TransientRegistration : RegistrationBase
    {
        private DataTypes.Lazy<Func<IocContainer, object>> construct;
        
        public override void Initialize(IocServiceRegistration serviceRegistration)
        {
            this.construct = new DataTypes.Lazy<Func<IocContainer, object>>(
                () => 
                    serviceRegistration.FactoryFunc ?? new ResolveFuncManager().GetOrCreateFunc(serviceRegistration.Key, serviceRegistration));
        }

        public override object ResolveService(IocContainer container, IocServiceRegistration serviceRegistration)
        {
            return construct.Value(container);
        }

        public override RegistrationBase Clone()
        {
            return new TransientRegistration()
            {
                IocServiceRegistration = IocServiceRegistration.Clone()
            };
        }
    }
}