using System.Collections.Concurrent;
using RadFramework.Libraries.Ioc.Factory;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc.Registrations
{
    public class TransientRegistration : RegistrationBase
    {
       
        private readonly IocContainer iocContainer;

        private readonly Lazy<Func<IocContainer, object>> construct;
        
        private static ConcurrentDictionary<IocKey, Func<IocContainer, object>> factoryCache = new ConcurrentDictionary<IocKey, Func<IocContainer, object>>();
        
        public TransientRegistration(IocKey key,
            ServiceFactoryLambdaGenerator lambdaGenerator, IocContainer iocContainer)
        {
            this.iocContainer = iocContainer;

            this.construct = new Lazy<Func<IocContainer, object>>(
                () => 
                    factoryCache.GetOrAdd(key ,
                        tuple => lambdaGenerator.CreateInstanceFactory(tImplementation, iocContainer, InjectionOptions)));
        }

        public override object ResolveService()
        {
            return construct.Value(iocContainer);
        }
    }
}