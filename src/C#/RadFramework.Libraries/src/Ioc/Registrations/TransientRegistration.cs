using System.Collections.Concurrent;
using RadFramework.Libraries.Ioc.Core;
using RadFramework.Libraries.Ioc.Factory;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc.Registrations
{
    public class TransientRegistration : RegistrationBase
    {
        private readonly DataTypes.Lazy<Func<Core.IocContainer, object>> construct;
        
        private static ConcurrentDictionary<IocKey, Func<Core.IocContainer, object>> factoryCache = new ConcurrentDictionary<IocKey, Func<Core.IocContainer, object>>();
        
        public TransientRegistration(
            IocKey key,
            Type tImplementation,
            ServiceFactoryLambdaGenerator lambdaGenerator)
        {

            this.construct = new DataTypes.Lazy<Func<Core.IocContainer, object>>(
                () => 
                    factoryCache.GetOrAdd(
                        key,
                        tuple => lambdaGenerator.CreateInstanceFactoryMethod(this,tImplementation, InjectionOptions)));
        }

        public override object ResolveService(IocContainer container)
        {
            return construct.Value(container);
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }
}