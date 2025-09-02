using System.Collections.Concurrent;
using RadFramework.Libraries.Ioc.Core;
using RadFramework.Libraries.Ioc.Factory;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc.Registrations
{
    public class TransientRegistration : RegistrationBase
    {
        private DataTypes.Lazy<Func<Core.IocContainer, object>> construct;
        
        private static ConcurrentDictionary<IocKey, Func<Core.IocContainer, object>> factoryCache = new ConcurrentDictionary<IocKey, Func<Core.IocContainer, object>>();
        
        public override void Initialize(IocServiceRegistration serviceRegistration)
        {
            this.construct = new DataTypes.Lazy<Func<Core.IocContainer, object>>(
                () => 
                    serviceRegistration.FactoryFunc ?? 
                    factoryCache.GetOrAdd(
                        serviceRegistration.Key,
                        tuple => ServiceFactoryLambdaGenerator.DefaultInstance.CreateInstanceFactoryMethod(serviceRegistration)));
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