using System.Collections.Concurrent;
using RadFramework.Libraries.Ioc.Core;
using RadFramework.Libraries.Ioc.Factory;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc.Registrations
{
    public class TransientRegistration : RegistrationBase
    {
       
        private readonly Core.IocContainer iocContainer;

        private readonly DataTypes.Lazy<Func<Core.IocContainer, object>> construct;
        
        private static ConcurrentDictionary<IocKey, Func<Core.IocContainer, object>> factoryCache = new ConcurrentDictionary<IocKey, Func<Core.IocContainer, object>>();
        
        public TransientRegistration(
            IocKey key,
            Type tImplementation,
            ServiceFactoryLambdaGenerator lambdaGenerator,
            Core.IocContainer iocContainer)
        {
            this.iocContainer = iocContainer;

            this.construct = new DataTypes.Lazy<Func<Core.IocContainer, object>>(
                () => 
                    factoryCache.GetOrAdd(
                        key,
                        tuple => lambdaGenerator.CreateInstanceFactoryMethod(this,tImplementation, iocContainer, InjectionOptions)));
        }

        public override object ResolveService()
        {
            return construct.Value(iocContainer);
        }
    }
}